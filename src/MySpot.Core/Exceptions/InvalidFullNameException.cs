namespace MySpot.Core.Exceptions;

public sealed class InvalidFullNameException(string fullName) : MySpotException($"Full name: '{fullName}' is invalid.");