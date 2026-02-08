using MySpot.Core.Exceptions;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities;

public class WeeklyParkingSpot
{
    private readonly HashSet<Reservation> _reservations = new();
    
    public ParkingSpotId Id { get; }
    public Week Week { get; }
    public ParkingSpotName Name { get; }
    public IEnumerable<Reservation> Reservations => _reservations;

    public WeeklyParkingSpot(ParkingSpotId id, Week week, ParkingSpotName name)
    {
        Id = id;
        Name = name;
        Week = week;
    }

    public void AddReservation(Reservation newReservation, Date now)
    {
        var isInvalidDate = newReservation.Date.Value < Week.From.Value ||
                            newReservation.Date.Value > Week.To.Value;
        
        if (isInvalidDate)
        {
            throw new InvalidReservationDateException((DateTime)newReservation.Date);
        }
        
        var reservationAlreadyExists = Reservations.Any(
            reservation => reservation.Date == newReservation.Date);
        if (reservationAlreadyExists)
        {
            throw new ParkingSpotAlreadyReservedException(Name, (DateTime)newReservation.Date);
        }
        
        _reservations.Add(newReservation);
    }

    public void RemoveReservation(Guid reservationId)
    {
        _reservations.RemoveWhere(reservation => reservation.Id == reservationId);
    }
}