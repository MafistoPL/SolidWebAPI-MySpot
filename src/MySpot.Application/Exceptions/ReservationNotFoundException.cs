using MySpot.Core.Exceptions;

namespace MySpot.Application.Exceptions;

public sealed class ReservationNotFoundException(Guid id) : MySpotException($"Reservation with id: {id} not found.");