using MySpot.Api.Entities;
using MySpot.Api.Exceptions;
using MySpot.Api.services;
using MySpot.Api.ValueObjects;
using Shouldly;

namespace MySpot.Tests.Unit.Entities;

public class WeeklyParkingSpotTests
{
    private Clock _clock = new Clock();

    [Fact]
    public void AddReservation_WithInvalidDate_ThrowsInvalidReservationDateException()
    {
        // Arrange
        var now = _clock.Current();
        var invalidDate = new Date(now.AddDays(8));
        var weeklyParkingSpotId = Guid.NewGuid();
        var weeklyParkingSpot = new WeeklyParkingSpot(weeklyParkingSpotId, new Week(now), "P1");
        var reservation = new Reservation(
            Guid.NewGuid(),
            weeklyParkingSpotId,
            "EmployeeName",
            "XYY-1234",
            invalidDate);
        
        // Act
        var exception = Record.Exception(
            () => weeklyParkingSpot.AddReservation(reservation, invalidDate));
        
        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidReservationDateException>();
    }
}