namespace MySpot.Api.Exceptions;

public sealed class ParkingSpotAlreadyReservedException(string name, DateTime date)
    : MySpotException($"Parking spot {name} is  already reserved at {date:yyyy-MM-dd}.");