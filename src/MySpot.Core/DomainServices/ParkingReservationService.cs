using MySpot.Core.Abstractions;
using MySpot.Core.Entities;
using MySpot.Core.Exceptions;
using MySpot.Core.Policies;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.DomainServices;

internal sealed class ParkingReservationService(
    IEnumerable<IReservationPolicy> reservationPolicies,
    IClock clock)
    : IParkingReservationService
{
    public void ReserveSpotForVehicle(
        IEnumerable<WeeklyParkingSpot> allParkingSpots, 
        string jobTitle, 
        WeeklyParkingSpot parkingSpotToReserve,
        Reservation reservation)
    {
        var parkingSpotId = parkingSpotToReserve.Id;
        var policy = reservationPolicies.SingleOrDefault(
            x => x.CanBeApplied(jobTitle));

        if (policy is null)
        {
            throw new NoReservationPolicyFoundException(jobTitle);
        }

        if (!policy.CanReserve(allParkingSpots, reservation.EmployeeName))
        {
            throw new CannotReserveParkingSpotException(parkingSpotId);
        }
        
        parkingSpotToReserve.AddReservation(reservation, new Date(clock.Current()));
    }
}