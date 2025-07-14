namespace MySpot.Api.Models;

public class Reservation
{
    public int Id { get; set; }
    public string EmployeeName { get; set; }
    public string ParkingSpotName { get; set; }
    public string LicensePlate { get; set; }
    public DateTime Date { get; set; }
}