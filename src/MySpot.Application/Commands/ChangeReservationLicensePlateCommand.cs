using MySpot.Core.ValueObjects;

namespace MySpot.Application.Commands;

public record ChangeReservationLicensePlateCommand(
    Guid ReservationId,
    LicensePlate LicensePlate);