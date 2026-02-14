using MySpot.Core.Exceptions;

namespace MySpot.Application.Exceptions;

public class ReservationNotFound(Guid id) : MySpotException($"Reservation with id: {id} not found.");