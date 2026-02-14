using MySpot.Core.ValueObjects;

namespace MySpot.Application.Commands;

public record ReserveParkingSpotForVehicleCommand(
    Guid ParkingSpotId,
    Guid ReservationId,
    ParkingSpotCapacityValue Capacity,
    DateTime Date,
    string EmployeeName,
    string LicensePlate);