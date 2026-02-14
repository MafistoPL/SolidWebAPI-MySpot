using MySpot.Core.Exceptions;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities;

public class VehicleReservation : Reservation
{
    public EmployeeName EmployeeName { get; private set; } = null!;
    public LicensePlate LicensePlate { get; private set; } = null!;
    
    private VehicleReservation()
    {
    }

    public VehicleReservation(ReservationId id, 
        ParkingSpotId parkingSpotId,
        ParkingSpotCapacity capacity,
        Date date, 
        Date now, 
        EmployeeName employeeName,
        LicensePlate licensePlate
        ) : base(id, parkingSpotId, capacity, date, now)
    {
        EmployeeName = employeeName;
        ChangeLicensePlate(licensePlate);
    }
    
    public void ChangeLicensePlate(LicensePlate licensePlate)
    {
        LicensePlate = licensePlate;
    }
}