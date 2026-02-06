using MySpot.Api.ValueObjects;

namespace MySpot.Api.Commands;

public record ChangeReservationLicensePlateCommand(
    Guid ReservationId,
    LicensePlate LicensePlate);