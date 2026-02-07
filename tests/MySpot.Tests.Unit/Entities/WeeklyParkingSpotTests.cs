using MySpot.Api.Entities;
using MySpot.Api.Exceptions;
using MySpot.Api.ValueObjects;
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
        var reservation = new Reservation(
            Guid.NewGuid(),
            _weeklyParkingSpot.Id,
            "EmployeeName",
            "XYY-1234",
            invalidDate,
            _now);
        
        // Act
        var exception = Record.Exception(
            () => _weeklyParkingSpot.AddReservation(reservation, invalidDate));
        
        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidReservationDateException>();
    }
    
    [Fact]
    public void AddReservation_WithAlreadyReservedDate_ThrowsParkingSpotAlreadyReservedException()
    {
        // Arrange
        var validDate = new Date(new DateTime(2022, 08, 11));
        var reservation = new Reservation(
            Guid.NewGuid(),
            _weeklyParkingSpot.Id,
            "EmployeeName",
            "XYY-1234",
            validDate,
            _now);
        _weeklyParkingSpot.AddReservation(reservation, validDate);
        
        // Act
        var exception = Record.Exception(
            () => _weeklyParkingSpot.AddReservation(reservation, validDate));
        
        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ParkingSpotAlreadyReservedException>();
    }

    [Fact]
    public void AddReservation_WithValidDate_AddsReservation()
    {
        // Arrange
        var validDate = new Date(new DateTime(2022, 08, 11));
        var reservation = new Reservation(
            Guid.NewGuid(),
            _weeklyParkingSpot.Id,
            "EmployeeName",
            "XYY-1234",
            validDate,
            _now);
        
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
        _weeklyParkingSpot = new WeeklyParkingSpot(Guid.NewGuid(), new Week(_now), "P1");
    }

    #endregion
}
