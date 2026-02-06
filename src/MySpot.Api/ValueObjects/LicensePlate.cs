using MySpot.Api.Exceptions;

namespace MySpot.Api.ValueObjects;

public class LicensePlate
{
    public string Value { get; }

    public LicensePlate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyLicensePlateException();
        }

        if (value.Length is < 5 or > 8)
        {
            throw new InvalidLicensePlateLengthException(value);
        }
        
        Value = value;
    }
    
    public static implicit operator string (LicensePlate licensePlate) => licensePlate.Value;
    public static implicit operator LicensePlate(string licensePlate) => new(licensePlate);
}