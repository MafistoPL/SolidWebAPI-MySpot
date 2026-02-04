using MySpot.Api.Models;

namespace MySpot.Api.services;

public class ReservationsService
{
    private static int _id = 1;
    private static readonly List<Reservation> Reservations = new();
    private static readonly List<string> ParkingSpotNames = ["P1", "P2", "P3", "P4", "P5"];

    public Reservation? Get(int id) 
        => Reservations.SingleOrDefault(x => x.Id == id);

    public IEnumerable<Reservation> Get() 
        => Reservations;

    public int? Create(Reservation reservation)
    {
        reservation.Date = DateTime.UtcNow.AddDays(1).Date;
        
        var parkingSpotDoesntExist = !ParkingSpotNames.Contains(reservation.ParkingSpotName);
        var reservationAlreadyExists = Reservations.Any(
            r => r.ParkingSpotName == reservation.ParkingSpotName &&
                r.Date == reservation.Date);

        if (reservationAlreadyExists || parkingSpotDoesntExist)
        {
            return null;
        }
        
        reservation.Id = _id++;
        Reservations.Add(reservation);

        return reservation.Id;
    }

    public bool Put(int id, Reservation reservation)
    {
        var existingReservation = Reservations.SingleOrDefault(x => x.Id == id);

        if (existingReservation == null) return false;
        
        existingReservation.LicensePlate = reservation.LicensePlate;

        return true;
    }

    public bool Delete(int id)
    {
        var existingReservation = Reservations.SingleOrDefault(x => x.Id == id);
        
        if (existingReservation == null) return false;
        
        Reservations.Remove(existingReservation);
        
        return true;
    }
}