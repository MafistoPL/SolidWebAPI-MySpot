namespace MySpot.Core.Exceptions;

public sealed class InvalidLicensePlateLengthException(string value)
    : MySpotException($"Invalid license plate length: {value}");