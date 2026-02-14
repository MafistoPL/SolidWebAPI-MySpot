using MySpot.Core.Exceptions;
using MySpot.Core.ValueObjects;
using Shouldly;

namespace MySpot.Tests.Unit.ValueObjects;

public class ParkingSpotCapacityTests
{
    [Theory]
    [InlineData(ParkingSpotCapacityValue.OneQuarter)]
    [InlineData(ParkingSpotCapacityValue.Half)]
    [InlineData(ParkingSpotCapacityValue.ThreeQuarter)]
    [InlineData(ParkingSpotCapacityValue.Full)]
    public void Ctor_WithValidValue_Succeeds(ParkingSpotCapacityValue value)
    {
        // Act
        var capacity = new ParkingSpotCapacity(value);

        // Assert
        ((ParkingSpotCapacityValue)capacity).ShouldBe(value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(5)]
    public void Ctor_WithInvalidValue_Throws(int rawValue)
    {
        // Arrange
        var value = (ParkingSpotCapacityValue)rawValue;

        // Act
        var exception = Record.Exception(() => new ParkingSpotCapacity(value));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidParkingSpotCapacityException>();
    }
}
