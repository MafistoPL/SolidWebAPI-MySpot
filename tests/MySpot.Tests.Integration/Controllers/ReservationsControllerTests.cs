using System.Net;
using System.Net.Http.Json;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Tests.Integration.Infrastructure;

namespace MySpot.Tests.Integration.Controllers;

public class ReservationsControllerTests : IClassFixture<ApplicationWebFactory>, IAsyncLifetime
{
    private static readonly Guid ParkingSpotId1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
    private static readonly Guid ParkingSpotId2 = Guid.Parse("00000000-0000-0000-0000-000000000002");
    private static readonly Guid ParkingSpotId3 = Guid.Parse("00000000-0000-0000-0000-000000000003");

    private readonly ApplicationWebFactory _factory;
    private HttpClient _backend = null!;
    private TestClock _clock = null!;

    public ReservationsControllerTests(ApplicationWebFactory factory)
    {
        _factory = factory;
    }

    public async Task InitializeAsync()
    {
        _clock = _factory.Clock;
        _clock.CurrentTime = new DateTime(2022, 08, 10, 12, 0, 0);
        await _factory.InitializeAsync();
        _backend = _factory.CreateClient();
    }

    public async Task DisposeAsync()
    {
        _backend.Dispose();
        await Task.CompletedTask;
    }

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        var response = await _backend.GetAsync("reservations");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var reservations = await response.Content.ReadFromJsonAsync<List<ReservationDto>>();
        Assert.NotNull(reservations);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_ForMissingReservation()
    {
        var response = await _backend.GetAsync($"reservations/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PostVehicle_CreatesReservation_AndGetByIdReturnsOk()
    {
        var reservationId = await CreateVehicleReservationAsync(ParkingSpotId1, _clock.Current());
        try
        {
            var response = await _backend.GetAsync($"reservations/{reservationId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var reservation = await response.Content.ReadFromJsonAsync<ReservationDto>();
            Assert.NotNull(reservation);
            Assert.Equal(reservationId, reservation!.Id);
        }
        finally
        {
            await DeleteReservationAsync(reservationId);
        }
    }

    [Fact]
    public async Task PostVehicle_ReturnsBadRequest_ForUnknownParkingSpot()
    {
        var command = new ReserveParkingSpotForVehicleCommand(
            Guid.NewGuid(),
            Guid.Empty,
            _clock.Current(),
            "Employee",
            "ABC123");

        var response = await _backend.PostAsJsonAsync("reservations/vehicle", command);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostCleaning_ReturnsOk_AndReplacesReservationsForDate()
    {
        var cleaningDate = new DateTime(2022, 08, 13);
        await DeleteReservationsForDateAsync(cleaningDate);
        var reservationId = await CreateVehicleReservationAsync(ParkingSpotId1, cleaningDate);

        try
        {
            var response = await _backend.PostAsJsonAsync(
                "reservations/cleaning",
                new ReserveParkingSpotForCleaningCommand(cleaningDate));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var reservations = await GetAllReservationsAsync();
            var reservationsForDate = reservations
                .Where(reservation => reservation.Date.Date == cleaningDate.Date)
                .ToList();

            Assert.Equal(3, reservationsForDate.Count);
            Assert.All(reservationsForDate, reservation =>
            {
                Assert.True(string.IsNullOrWhiteSpace(reservation.EmployeeName));
                Assert.True(string.IsNullOrWhiteSpace(reservation.LicensePlate));
            });
        }
        finally
        {
            await DeleteReservationsForDateAsync(cleaningDate);
            await DeleteReservationIfExistsAsync(reservationId);
        }
    }

    [Fact]
    public async Task PostCleaning_ReturnsBadRequest_ForPastDate()
    {
        var command = new ReserveParkingSpotForCleaningCommand(
            _clock.Current().AddDays(-1));

        var response = await _backend.PostAsJsonAsync("reservations/cleaning", command);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_ReturnsOk_ForExistingReservation()
    {
        var reservationId = await CreateVehicleReservationAsync(ParkingSpotId2, _clock.Current());
        try
        {
            var response = await UpdateLicensePlateAsync(reservationId, "XYZ987");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        finally
        {
            await DeleteReservationAsync(reservationId);
        }
    }

    [Fact]
    public async Task Put_ReturnsNotFound_ForMissingReservation()
    {
        var response = await UpdateLicensePlateAsync(Guid.NewGuid(), "XYZ987");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_ForExistingReservation()
    {
        var reservationId = await CreateVehicleReservationAsync(ParkingSpotId3, _clock.Current());

        var response = await DeleteReservationAsync(reservationId);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_ForMissingReservation()
    {
        var response = await DeleteReservationAsync(Guid.NewGuid());

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private async Task<Guid> CreateVehicleReservationAsync(Guid parkingSpotId, DateTime date)
    {
        var command = new ReserveParkingSpotForVehicleCommand(
            parkingSpotId,
            Guid.Empty,
            date,
            "Employee",
            "ABC123");

        var response = await _backend.PostAsJsonAsync("reservations/vehicle", command);
        if (response.StatusCode != HttpStatusCode.Created)
        {
            var body = await response.Content.ReadAsStringAsync();
            Assert.Fail($"Expected Created but got {response.StatusCode}. Body: {body}");
        }

        var location = response.Headers.Location?.ToString();
        Assert.False(string.IsNullOrWhiteSpace(location));
        var idSegment = location!.Split('/').Last();
        return Guid.Parse(idSegment);
    }

    private async Task<List<ReservationDto>> GetAllReservationsAsync()
    {
        var response = await _backend.GetAsync("reservations");
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            Assert.Fail($"Expected OK but got {response.StatusCode}. Body: {body}");
        }

        var reservations = await response.Content.ReadFromJsonAsync<List<ReservationDto>>();
        return reservations ?? new List<ReservationDto>();
    }

    private Task<HttpResponseMessage> UpdateLicensePlateAsync(Guid reservationId, string licensePlate)
    {
        var payload = new
        {
            reservationId,
            licensePlate
        };

        return _backend.PutAsJsonAsync("reservations", payload);
    }

    private Task<HttpResponseMessage> DeleteReservationAsync(Guid reservationId)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, "reservations")
        {
            Content = JsonContent.Create(new DeleteReservationCommand(reservationId))
        };

        return _backend.SendAsync(request);
    }

    private async Task DeleteReservationIfExistsAsync(Guid reservationId)
    {
        var response = await DeleteReservationAsync(reservationId);
        if (response.StatusCode != HttpStatusCode.NoContent &&
            response.StatusCode != HttpStatusCode.NotFound)
        {
            var body = await response.Content.ReadAsStringAsync();
            Assert.Fail($"Expected cleanup to return NoContent or NotFound but got {response.StatusCode}. Body: {body}");
        }
    }

    private async Task DeleteReservationsForDateAsync(DateTime date)
    {
        var reservations = await GetAllReservationsAsync();
        var reservationsForDate = reservations
            .Where(reservation => reservation.Date.Date == date.Date)
            .Select(reservation => reservation.Id)
            .ToList();

        foreach (var reservationId in reservationsForDate)
        {
            await DeleteReservationIfExistsAsync(reservationId);
        }
    }
}
