using MySpot.Core.Exceptions;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities;

public class Reservation
{
    public ReservationId Id { get; private set; }
    public ParkingSpotId ParkingSpotId { get; private set; }
    public EmployeeName EmployeeName { get; private set; }
    public LicensePlate LicensePlate { get; private set; }
    public Date Date { get; private set; }

    private Reservation()
    {
    }

    public Reservation(ReservationId id,
        ParkingSpotId parkingSpotId,
        EmployeeName employeeName, 
        LicensePlate licensePlate, 
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
        var reservationDay = reservationDate.Value.Date;
        var currentDay = now.Value.Date;
        if (reservationDay < currentDay)
        {
            throw new InvalidReservationDateException((DateTime)reservationDate);
        }

        Date = new Date(new DateTimeOffset(reservationDay, reservationDate.Value.Offset));
    }
}
