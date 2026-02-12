using MySpot.Core.Entities;
using MySpot.Core.Exceptions;
using MySpot.Core.ValueObjects;
using Shouldly;

namespace MySpot.Tests.Unit.Entities;

public class ReservatonTests
{
    [Theory]
    [InlineData("2022-08-09")]
    public void VehicleReservationCtor_WithPassedDate_ThrowsInvalidReservationDateException(string dateString)
    {
        // Arrange
        var nowValue = new DateTime(2022, 08, 10);
        var now = new Date(nowValue);
        var invalidDate = new Date(DateTime.Parse(dateString));
        var weeklyParkingSpotId = Guid.NewGuid();
        
        // Act
        var exception = Record.Exception(
            () => new VehicleReservation(
                Guid.NewGuid(),
                weeklyParkingSpotId,
                invalidDate,
                now,
                "EmployeeName",
                "XYY-1234"));
        
        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidReservationDateException>();
    }

    [Theory]
    [InlineData("2022-08-09")]
    public void CleaningReservationCtor_WithPassedDate_ThrowsInvalidReservationDateException(string dateString)
    {
        // Arrange
        var nowValue = new DateTime(2022, 08, 10);
        var now = new Date(nowValue);
        var invalidDate = new Date(DateTime.Parse(dateString));
        var weeklyParkingSpotId = Guid.NewGuid();

        // Act
        var exception = Record.Exception(
            () => new CleaningReservation(
                Guid.NewGuid(),
                weeklyParkingSpotId,
                invalidDate,
                now));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidReservationDateException>();
    }
}
