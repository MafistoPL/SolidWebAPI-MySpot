using MySpot.Api.Entities;
using MySpot.Api.Exceptions;
using MySpot.Api.services;
using MySpot.Api.ValueObjects;
using Shouldly;

namespace MySpot.Tests.Unit.Entities;

public class ReservatonTests
{
    private Clock _clock = new Clock();

    [Theory]
    [InlineData("2022-08-09")]
    public void ReservationCtor_WithPassedDate_ThrowsInvalidReservationDateException(string dateString)
    {
        // Arrange
        var now = new Date(new DateTime(2022, 08, 10));
        var invalidDate = new Date(DateTime.Parse(dateString));
        var weeklyParkingSpotId = Guid.NewGuid();
        
        // Act
        var exception = Record.Exception(
            () => new Reservation(
                Guid.NewGuid(),
                weeklyParkingSpotId,
                "EmployeeName",
                "XYY-1234",
                invalidDate,
                now));
        
        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidReservationDateException>();
    }
}