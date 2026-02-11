using MySpot.Core.Entities;

namespace MySpot.Core.DomainServices;

public interface IParkingReservationService
{
    void ReserveSpotForVehicle(
        IEnumerable<WeeklyParkingSpot> allParkingSpots, 
        string jobTitle,
        WeeklyParkingSpot parkingSpotToReserve, 
        Reservation reservation);
}