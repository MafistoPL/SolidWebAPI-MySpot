using MySpot.Api.Exceptions;

namespace MySpot.Api.Entities;

public class WeeklyParkingSpot
{
    private readonly HashSet<Reservation> _reservations = new();
    
    public Guid Id { get; }
    public DateTime From { get; }
    public DateTime To { get; }
    public string Name { get; }
    public IEnumerable<Reservation> Reservations => _reservations;

    public WeeklyParkingSpot(Guid id, DateTime from, DateTime to, string name)
    {
        Id = id;
        From = from;
        To = to;
        Name = name;
    }

    public void AddReservation(Reservation newReservation, DateTime now)
    {
        var isInvalidDate = newReservation.Date.Date < From.Date ||
            newReservation.Date.Date > To.Date ||
            newReservation.Date.Date < now.Date;
        
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