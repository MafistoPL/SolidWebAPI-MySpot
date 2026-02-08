namespace MySpot.Application.Commands;

public record DeleteReservationCommand(
    Guid ReservationId);