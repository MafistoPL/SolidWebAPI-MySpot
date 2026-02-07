using MySpot.Api.Commands;
using MySpot.Api.Entities;
using MySpot.Api.Repository;
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
        var parkingSpot = _weeklyParkingSpotRepository.GetAll().First();
        var command = new CreateReservationCommand(
            parkingSpot.Id,
            Guid.NewGuid(),
            _clock.Current().Date,
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

    private readonly TestClock _clock = new(new DateTime(2022, 08, 10));
    
    private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;
    public ReservationsServiceTests()
    {
        _weeklyParkingSpotRepository = new InMemoryWeeklyParkingSpotRepository(_clock);
        _reservationsService = new ReservationsService(_weeklyParkingSpotRepository, _clock);
    }
    
    #endregion
}
