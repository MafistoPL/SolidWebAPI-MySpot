using MySpot.Core.Exceptions;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities;

public class  WeeklyParkingSpot
{
    private readonly HashSet<Reservation> _reservations = new();
    
    public ParkingSpotId Id { get; private set; }
    public Week Week { get; private set; }
    public ParkingSpotCapacity Capacity { get; private set; }
    public ParkingSpotName Name { get; private set; }
    public IEnumerable<Reservation> Reservations => _reservations;

    private WeeklyParkingSpot()
    {
        
    }
    
    private WeeklyParkingSpot(ParkingSpotId id, Week week, ParkingSpotName name, ParkingSpotCapacity capacity)
    {
        Id = id;
        Name = name;
        Week = week;
        Capacity = capacity;
    }
    
    public static WeeklyParkingSpot Create(ParkingSpotId id, Week week, ParkingSpotName name)
    {
        return new WeeklyParkingSpot(id, week, name, ParkingSpotCapacityValue.Full);
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
        
        var dateCapacity = _reservations
            .Where(reservation => reservation.Date.Value.Date == reservationDay)
            .Sum(x => (int)x.Capacity.Value);
        
        if (dateCapacity + (int)newReservation.Capacity.Value > (int)Capacity.Value)
        {
            throw new ParkingSpotCapacityExceededException(Id);
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
