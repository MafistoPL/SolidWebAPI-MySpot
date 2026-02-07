using MySpot.Api.Commands;
using MySpot.Api.Entities;
using MySpot.Api.services;
using MySpot.Api.ValueObjects;
using Shouldly;
using MySpot.Tests.Unit.Infrastructure;

namespace MySpot.Tests.Unit.Services;

public class ReservationsServiceTests
{
    [Fact]
    public void Create_WithCorrectDate_ShouldSucceed()
    {
        // Arrange
        var parkingSpot = WeeklyParkingSpots.First();
        var command = new CreateReservationCommand(
            parkingSpot.Id,
            Guid.NewGuid(),
            Clock.Current().Date,
            "John Doe",
            "ABC-123"
            );
        
        // Act
        var reservationId = _reservationsService.Create(command);

        // Assert
        reservationId.ShouldNotBeNull();
        reservationId.Value.ShouldBe(command.ReservationId);
    }
    
    #region Arrange
    
    private readonly ReservationsService _reservationsService;

    private static readonly TestClock Clock = new()
    {
        CurrentTime = new DateTime(2022, 08, 10)
    };
    // NOTE: This shared list intentionally leaks state between tests; we will refactor to isolate data.
    private static readonly List<WeeklyParkingSpot> WeeklyParkingSpots = [
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000001"), 
            new Week(Clock.Current()), "P1"),
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000002"),
            new Week(Clock.Current()), "P2"),
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000003"),
            new Week(Clock.Current()), "P3"),
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000004"),
            new Week(Clock.Current()), "P4"),
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000005"), 
            new Week(Clock.Current()), "P5")
    ];

    public ReservationsServiceTests()
    {
        _reservationsService = new ReservationsService(WeeklyParkingSpots, Clock);
    }
    
    #endregion
}
