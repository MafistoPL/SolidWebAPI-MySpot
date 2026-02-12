using MySpot.Core.Exceptions;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities;

public class  WeeklyParkingSpot
{
    private readonly HashSet<Reservation> _reservations = new();
    
    public ParkingSpotId Id { get; private set; }
    public Week Week { get; private set; }
    public ParkingSpotName Name { get; private set; }
    public IEnumerable<Reservation> Reservations => _reservations;

    public WeeklyParkingSpot()
    {
        
    }
    
    public WeeklyParkingSpot(ParkingSpotId id, Week week, ParkingSpotName name)
    {
        Id = id;
        Name = name;
        Week = week;
    }

    internal void AddReservation(Reservation newReservation, Date now)
    {
        var reservationDay = newReservation.Date.Value.Date;
        var weekFromDay = Week.From.Value.Date;
        var weekToDay = Week.To.Value.Date;
        var isInvalidDate = reservationDay < weekFromDay || reservationDay > weekToDay;
        
        if (isInvalidDate)
        {
            throw new InvalidReservationDateException(reservationDay, weekFromDay, weekToDay);
        }
        
        var reservationAlreadyExists = Reservations.Any(
            reservation => reservation.Date == newReservation.Date);
        if (reservationAlreadyExists)
        {
            throw new ParkingSpotAlreadyReservedException(Name, (DateTime)newReservation.Date);
        }
        
        _reservations.Add(newReservation);
    }

    public void RemoveReservation(ReservationId reservationId)
    {
        _reservations.RemoveWhere(reservation => reservation.Id == reservationId);
    }
    
    public void RemoveReservations(IEnumerable<Reservation> reservations)
    {
        _reservations.RemoveWhere(
            reservation => reservations.Any(
                r => r.Id == reservation.Id));
    }
}
