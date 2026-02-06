using MySpot.Api.Exceptions;
using MySpot.Api.ValueObjects;

namespace MySpot.Api.Entities;

public class Reservation
{
    public Guid Id { get; }
    public Guid ParkingSpotId { get; set; }
    public string EmployeeName { get; private set; }
    public string LicensePlate { get; private set; }
    public Date Date { get; private set; }

    public Reservation(Guid id,
        Guid parkingSpotId,
        string employeeName, 
        string licensePlate, 
        Date date)
    {
        Id = id;
        ParkingSpotId = parkingSpotId;
        EmployeeName = employeeName;
        ChangeLicensePlate(licensePlate);
        Date = date;
    }

    public void ChangeLicensePlate(LicensePlate licensePlate)
    {
        if (string.IsNullOrWhiteSpace(licensePlate))
        {
            throw new EmptyLicensePlateException();
        }
        
        LicensePlate = licensePlate;
    }
}