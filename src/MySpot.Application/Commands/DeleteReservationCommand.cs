using MySpot.Application.Abstractions;

namespace MySpot.Application.Commands;

public record DeleteReservationCommand(
    Guid ReservationId) : ICommand;