namespace MySpot.Core.Exceptions;

public sealed class InvalidReservationDateException : MySpotException
{
    public InvalidReservationDateException(DateTime date)
        : base($"Reservation date {date:yyyy-MM-dd} is invalid")
    {
    }

    public InvalidReservationDateException(DateTime date, DateTime from, DateTime to)
        : base($"Reservation date {date:yyyy-MM-dd} is invalid (valid range {from:yyyy-MM-dd} - {to:yyyy-MM-dd})")
    {
    }
}
