using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.DomainServices;

public interface IParkingReservationService
{
    void ReserveSpotForVehicle(
        IEnumerable<WeeklyParkingSpot> allParkingSpots, 
        string jobTitle,
        WeeklyParkingSpot parkingSpotToReserve, 
        VehicleReservation reservation);

    void ReserveParkingForCleaning(IEnumerable<WeeklyParkingSpot> allParkingSpots, Date date);
}