using MySpot.Core.Entities;
using MySpot.Core.Exceptions;
using MySpot.Core.ValueObjects;
using Shouldly;

namespace MySpot.Tests.Unit.Entities;

public class WeeklyParkingSpotTests
{
    [Theory]
    [InlineData("2022-08-17")]
    [InlineData("2022-08-27")]
    public void AddReservation_WithInvalidDate_ThrowsInvalidReservationDateException(string dateString)
    {
        // Arrange
        var invalidDate = new Date(DateTime.Parse(dateString));
        var reservation = new VehicleReservation(
            Guid.NewGuid(),
            _weeklyParkingSpot.Id,
            ParkingSpotCapacityValue.Full,
            invalidDate,
            _now,
            "EmployeeName",
            "XYY-1234");
        
        // Act
        var exception = Record.Exception(
            () => _weeklyParkingSpot.AddReservation(reservation, _now));
        
        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidReservationDateException>();
    }
    
    [Fact]
    public void AddReservation_WithAlreadyReservedDate_ThrowsParkingSpotCapacityExceededException()
    {
        // Arrange
        var validDate = new Date(new DateTime(2022, 08, 11));
        var reservation = new VehicleReservation(
            Guid.NewGuid(),
            _weeklyParkingSpot.Id,
            ParkingSpotCapacityValue.Full,
            validDate,
            _now,
            "EmployeeName",
            "XYY-1234");
        _weeklyParkingSpot.AddReservation(reservation, validDate);
        
        // Act
        var exception = Record.Exception(
            () => _weeklyParkingSpot.AddReservation(reservation, validDate));
        
        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ParkingSpotCapacityExceededException>();
    }

    [Fact]
    public void AddReservation_WithValidDate_AddsReservation()
    {
        // Arrange
        var validDate = new Date(new DateTime(2022, 08, 11));
        var reservation = new VehicleReservation(
            Guid.NewGuid(),
            _weeklyParkingSpot.Id,
            ParkingSpotCapacityValue.Full,
            validDate,
            _now,
            "EmployeeName",
            "XYY-1234");
        
        // Act
        _weeklyParkingSpot.AddReservation(reservation, validDate);
        
        // Assert
        _weeklyParkingSpot.Reservations.ShouldHaveSingleItem();
        _weeklyParkingSpot.Reservations.ShouldContain(reservation);
    }
    
    #region Arrange

    private readonly Date _now;
    private readonly WeeklyParkingSpot _weeklyParkingSpot;

    public WeeklyParkingSpotTests()
    {
        _now = new Date(new DateTime(2022, 08, 10));
        _weeklyParkingSpot = WeeklyParkingSpot.Create(Guid.NewGuid(), new Week(_now), "P1");
    }

    #endregion
}
