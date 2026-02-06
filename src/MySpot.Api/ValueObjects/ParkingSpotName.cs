using MySpot.Api.Exceptions;

namespace MySpot.Api.ValueObjects;

public sealed record ParkingSpotName
{
    public string Value { get; }

    public ParkingSpotName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidParkingSpotNameException();
        }

        Value = value;
    }

    public static implicit operator string(ParkingSpotName name)
        => name.Value;

    public static implicit operator ParkingSpotName(string value)
        => new(value);
}
