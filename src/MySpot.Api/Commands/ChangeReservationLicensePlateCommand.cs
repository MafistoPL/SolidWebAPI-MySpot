namespace MySpot.Api.Commands;

public record ChangeReservationLicensePlateCommand(
    Guid ReservationId,
    string LicensePlate);