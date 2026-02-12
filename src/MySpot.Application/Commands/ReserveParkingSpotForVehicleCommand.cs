namespace MySpot.Application.Commands;

public record ReserveParkingSpotForVehicleCommand(
    Guid ParkingSpotId,
    Guid ReservationId,
    DateTime Date,
    string EmployeeName,
    string LicensePlate);