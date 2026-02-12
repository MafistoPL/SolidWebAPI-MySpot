using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities;

public class CleaningReservation : Reservation
{
    private CleaningReservation()
    {
    }
    
    public CleaningReservation(ReservationId id, ParkingSpotId parkingSpotId, Date date, Date now) 
        : base(id, parkingSpotId, date, now)
    {
    }
}