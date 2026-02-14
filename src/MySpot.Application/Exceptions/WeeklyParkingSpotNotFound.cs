using MySpot.Core.Exceptions;

namespace MySpot.Application.Exceptions;

public sealed class WeeklyParkingSpotNotFound(Guid id) 
    : MySpotException($"Weekly parking spot with id: {id} not found.")
{
}