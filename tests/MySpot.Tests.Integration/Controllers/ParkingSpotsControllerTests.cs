using System.Net;
using System.Net.Http.Json;
using MySpot.Api.Controllers;
using MySpot.Application.DTO;
using MySpot.Tests.Integration.Infrastructure;

namespace MySpot.Tests.Integration.Controllers;

public class ParkingSpotsControllerTests : IClassFixture<ApplicationWebFactory>, IAsyncLifetime
{
    private readonly ApplicationWebFactory _factory;
    private HttpClient _backend = null!;
    private TestClock _clock = null!;

    public ParkingSpotsControllerTests(ApplicationWebFactory factory)
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
        var response = await _backend.GetAsync($"{ParkingSpotsController.Path}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var parkingSpots = await response.Content.ReadFromJsonAsync<List<WeeklyParkingSpotDto>>();
        Assert.NotNull(parkingSpots);
    }
}
