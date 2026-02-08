namespace MySpot.Core.Exceptions;

public sealed class EmptyLicensePlateException() : MySpotException("License plate is invalid")
{
    
}