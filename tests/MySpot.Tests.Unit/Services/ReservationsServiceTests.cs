using MySpot.Application.Commands;
using MySpot.Application.services;
using MySpot.Core.DomainServices;
using MySpot.Core.Entities;
using MySpot.Core.Policies;
using MySpot.Core.Repositories;
using MySpot.Infrastructure.DAL.Repository;
using Shouldly;
using MySpot.Tests.Unit.Infrastructure;

namespace MySpot.Tests.Unit.Services;

public class ReservationsServiceTests
{
    [Fact]
    public async Task ReserveForVehicleAsync_WithCorrectDate_ShouldSucceed()
    {
        // Arrange
        var parkingSpot = (await _weeklyParkingSpotRepository.GetAllAsync()).First();
        var command = new ReserveParkingSpotForVehicleCommand(
            parkingSpot.Id,
            Guid.NewGuid(),
            _clock.Current().Date,
            "John Doe",
            "ABC-123"
            );
        
        // Act
        var reservationId = await _reservationsService.ReserveForVehicleAsync(command);

        // Assert
        reservationId.ShouldNotBeNull();
        reservationId.Value.ShouldBe(command.ReservationId);
    }

    [Fact]
    public async Task ReserveForVehicleAsync_ReturnsNull_ForUnknownParkingSpot()
    {
        // Arrange
        var command = new ReserveParkingSpotForVehicleCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            _clock.Current().Date,
            "John Doe",
            "ABC-123");
        
        // Act
        var reservationId = await _reservationsService.ReserveForVehicleAsync(command);

        // Assert
        reservationId.ShouldBeNull();
    }

    [Fact]
    public async Task ReserveForCleaningAsync_ReplacesReservationsForDate()
    {
        // Arrange
        var cleaningDate = _clock.Current().Date.AddDays(2);
        var parkingSpot = (await _weeklyParkingSpotRepository.GetAllAsync()).First();
        var vehicleCommand = new ReserveParkingSpotForVehicleCommand(
            parkingSpot.Id,
            Guid.NewGuid(),
            cleaningDate,
            "John Doe",
            "ABC-123");

        var vehicleReservationId = await _reservationsService.ReserveForVehicleAsync(vehicleCommand);
        vehicleReservationId.ShouldNotBeNull();

        // Act
        await _reservationsService.ReserveForCleaningAsync(
            new ReserveParkingSpotForCleaningCommand(cleaningDate));

        // Assert
        var weeklyParkingSpots = (await _weeklyParkingSpotRepository.GetAllAsync()).ToList();
        foreach (var weeklyParkingSpot in weeklyParkingSpots)
        {
            var reservationsForDate = weeklyParkingSpot.Reservations
                .Where(reservation => reservation.Date.Value.Date == cleaningDate.Date)
                .ToList();

            reservationsForDate.ShouldHaveSingleItem();
            reservationsForDate.Single().ShouldBeOfType<CleaningReservation>();
        }
    }

    [Fact]
    public async Task ChangeReservationLicensePlateAsync_ReturnsFalse_ForCleaningReservation()
    {
        // Arrange
        var cleaningDate = _clock.Current().Date.AddDays(1);
        await _reservationsService.ReserveForCleaningAsync(
            new ReserveParkingSpotForCleaningCommand(cleaningDate));

        var weeklyParkingSpot = (await _weeklyParkingSpotRepository.GetAllAsync()).First();
        var cleaningReservationId = weeklyParkingSpot.Reservations
            .OfType<CleaningReservation>()
            .Single(reservation => reservation.Date.Value.Date == cleaningDate.Date)
            .Id.Value;

        var command = new ChangeReservationLicensePlateCommand(
            cleaningReservationId,
            "XYZ-987");

        // Act
        var result = await _reservationsService.ChangeReservationLicensePlateAsync(command);

        // Assert
        result.ShouldBeFalse();
    }
    
    #region Arrange
    
    private readonly ReservationsService _reservationsService;

    private readonly TestClock _clock = new(new DateTime(2022, 08, 10, 12, 0, 0));
    
    private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;
    private readonly IReservationRepository _reservationRepository;
    public ReservationsServiceTests()
    {
        _weeklyParkingSpotRepository = new InMemoryWeeklyParkingSpotRepository(_clock);
        _reservationRepository = new InMemoryReservationRepository();

        IEnumerable<IReservationPolicy> reservationPolicies =
        [
            new RegularEmployeeReservationPolicy(_clock),
            new ManagerReservationPolicy(),
            new BossReservationPolicy()
        ];
            
        ParkingReservationService parkingReservationService 
            = new ParkingReservationService(reservationPolicies, _clock);
        
        _reservationsService = new ReservationsService(
            _weeklyParkingSpotRepository,
            _reservationRepository,
            parkingReservationService,
            _clock);
    }
    
    #endregion
}
