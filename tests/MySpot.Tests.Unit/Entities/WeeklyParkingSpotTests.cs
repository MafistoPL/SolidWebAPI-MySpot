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
    public void AddReservation_InvalidDate_ThrowsInvalidReservationDateException(string dateString)
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
    public void AddReservation_FullCapacity_ThrowsParkingSpotCapacityExceededException()
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
    public void AddReservation_ValidDate_AddsReservation()
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

    [Fact]
    public void AddReservation_CapacityWithinLimit_AddsMultipleReservations()
    {
        // Arrange
        var validDate = new Date(new DateTime(2022, 08, 12));
        var first = new VehicleReservation(
            Guid.NewGuid(),
            _weeklyParkingSpot.Id,
            ParkingSpotCapacityValue.Half,
            validDate,
            _now,
            "EmployeeName",
            "AAA-111");
        var second = new VehicleReservation(
            Guid.NewGuid(),
            _weeklyParkingSpot.Id,
            ParkingSpotCapacityValue.Half,
            validDate,
            _now,
            "EmployeeName",
            "BBB-222");

        // Act
        _weeklyParkingSpot.AddReservation(first, validDate);
        _weeklyParkingSpot.AddReservation(second, validDate);

        // Assert
        _weeklyParkingSpot.Reservations.Count().ShouldBe(2);
    }

    [Fact]
    public void AddReservation_CapacitySumExceeded_ThrowsParkingSpotCapacityExceededException()
    {
        // Arrange
        var validDate = new Date(new DateTime(2022, 08, 13));
        var first = new VehicleReservation(
            Guid.NewGuid(),
            _weeklyParkingSpot.Id,
            ParkingSpotCapacityValue.Half,
            validDate,
            _now,
            "EmployeeName",
            "CCC-333");
        var second = new VehicleReservation(
            Guid.NewGuid(),
            _weeklyParkingSpot.Id,
            ParkingSpotCapacityValue.Half,
            validDate,
            _now,
            "EmployeeName",
            "DDD-444");
        var third = new VehicleReservation(
            Guid.NewGuid(),
            _weeklyParkingSpot.Id,
            ParkingSpotCapacityValue.OneQuarter,
            validDate,
            _now,
            "EmployeeName",
            "EEE-555");

        _weeklyParkingSpot.AddReservation(first, validDate);
        _weeklyParkingSpot.AddReservation(second, validDate);

        // Act
        var exception = Record.Exception(
            () => _weeklyParkingSpot.AddReservation(third, validDate));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ParkingSpotCapacityExceededException>();
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
