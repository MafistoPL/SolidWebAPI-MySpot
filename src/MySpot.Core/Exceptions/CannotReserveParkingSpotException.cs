using MySpot.Core.ValueObjects;

namespace MySpot.Core.Exceptions;

public sealed class CannotReserveParkingSpotException(ParkingSpotId parkingSpotId) 
    : MySpotException($"Cannot reserve parking spot for employee: {parkingSpotId}")
{
    
}