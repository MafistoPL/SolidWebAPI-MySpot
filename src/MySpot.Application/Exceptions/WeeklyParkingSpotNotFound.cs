using MySpot.Core.Exceptions;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Exceptions;

public sealed class WeeklyParkingSpotNotFound : MySpotException
{
    public WeeklyParkingSpotNotFound(ParkingSpotId id) : base($"Weekly parking spot with id: {id} not found.")
    {
    }

    public WeeklyParkingSpotNotFound(ReservationId id) : base(
        $"Weekly parking spot for reservation with id: {id} not found.")
    {
    }
}