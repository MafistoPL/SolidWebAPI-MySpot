using MySpot.Core.Exceptions;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities;

public abstract class Reservation
{
    public ReservationId Id { get; private set; } = null!;
    public ParkingSpotId ParkingSpotId { get; private set; } = null!;
    public ParkingSpotCapacity Capacity { get; private set; } = null!;
    public Date Date { get; private set; } = null!;

    protected Reservation()
    {
    }

    public Reservation(ReservationId id,
        ParkingSpotId parkingSpotId,
        ParkingSpotCapacity capacity,
        Date date,
        Date now)
    {
        Id = id;
        ParkingSpotId = parkingSpotId;
        SetDate(date, now);
        Capacity = capacity;
    }

    public void SetDate(Date reservationDate, Date now)
    {
        var reservationDay = reservationDate.Value.Date;
        var currentDay = now.Value.Date;
        if (reservationDay < currentDay)
        {
            throw new InvalidReservationDateException((DateTime)reservationDate);
        }

        Date = new Date(new DateTimeOffset(reservationDay, reservationDate.Value.Offset));
    }
}
