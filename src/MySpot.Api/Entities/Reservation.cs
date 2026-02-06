using MySpot.Api.Exceptions;
using MySpot.Api.ValueObjects;

namespace MySpot.Api.Entities;

public class Reservation
{
    private static readonly TimeSpan MaxClientClockLag = TimeSpan.FromSeconds(5);
    public Guid Id { get; }
    public Guid ParkingSpotId { get; set; }
    public string EmployeeName { get; private set; }
    public string LicensePlate { get; private set; }
    public Date Date { get; private set; }

    public Reservation(Guid id,
        Guid parkingSpotId,
        string employeeName, 
        string licensePlate, 
        Date date,
        Date now)
    {
        Id = id;
        ParkingSpotId = parkingSpotId;
        EmployeeName = employeeName;
        ChangeLicensePlate(licensePlate);
        SetDate(date, now);
    }

    public void ChangeLicensePlate(LicensePlate licensePlate)
    {
        if (string.IsNullOrWhiteSpace(licensePlate))
        {
            throw new EmptyLicensePlateException();
        }
        
        LicensePlate = licensePlate;
    }

    public void SetDate(Date reservationDate, Date now)
    {
        // “We allow the client’s ‘now’ to be slightly ahead of the server’s ‘now’.”
        var earliestAllowedTime = now.Value - MaxClientClockLag;
        if (reservationDate.Value < earliestAllowedTime)
        {
            throw new InvalidReservationDateException((DateTime)reservationDate);
        }
        
        Date = reservationDate;
    }
}
