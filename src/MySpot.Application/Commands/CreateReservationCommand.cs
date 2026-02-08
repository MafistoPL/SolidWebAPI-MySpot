namespace MySpot.Application.Commands;

public record CreateReservationCommand(
    Guid ParkingSpotId,
    Guid ReservationId,
    DateTime Date,
    string EmployeeName,
    string LicensePlate);