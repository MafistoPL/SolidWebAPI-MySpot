namespace MySpot.Api.Exceptions;

public sealed class InvalidReservationDateException(DateTime date)
    : MySpotException($"Reservation date {date:yyyy-MM-dd} is invalid");