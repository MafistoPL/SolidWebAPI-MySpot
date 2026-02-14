using MySpot.Core.Exceptions;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Exceptions;

public sealed class WeeklyParkingSpotNotFoundException : MySpotException
{
    public WeeklyParkingSpotNotFoundException(ParkingSpotId id) : base($"Weekly parking spot with id: {id} not found.")
    {
    }

    public WeeklyParkingSpotNotFoundException(ReservationId id) : base(
        $"Weekly parking spot for reservation with id: {id} not found.")
    {
    }
}