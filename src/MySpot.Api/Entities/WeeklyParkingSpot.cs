using MySpot.Api.Exceptions;
using MySpot.Api.ValueObjects;

namespace MySpot.Api.Entities;

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
        var isInvalidDate = newReservation.Date.Date < Week.From.Value ||
            newReservation.Date.Date > Week.To.Value ||
            newReservation.Date.Date.AddDays(1) < now.Value;
        
        if (isInvalidDate)
        {
            throw new InvalidReservationDateException(newReservation.Date);
        }
        
        var reservationAlreadyExists = Reservations.Any(
            reservation => reservation.Date == newReservation.Date);
        if (reservationAlreadyExists)
        {
            throw new ParkingSpotAlreadyReservedException(Name, newReservation.Date);
        }
        
        _reservations.Add(newReservation);
    }

    public void RemoveReservation(Guid reservationId)
    {
        _reservations.RemoveWhere(reservation => reservation.Id == reservationId);
    }
}