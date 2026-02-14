using MySpot.Core.ValueObjects;

namespace MySpot.Core.Exceptions;

public sealed class InvalidParkingSpotCapacityException(ParkingSpotCapacityValue value) 
    : MySpotException($"Parking spot capacity: {value} is invalid.");