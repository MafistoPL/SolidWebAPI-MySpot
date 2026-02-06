namespace MySpot.Api.Exceptions;

public sealed class InvalidLicensePlateLengthException(string value)
    : MySpotException($"Invalid license plate length: {value}");