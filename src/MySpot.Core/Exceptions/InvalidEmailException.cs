namespace MySpot.Core.Exceptions;

public sealed class InvalidEmailException(string email) : MySpotException($"Email: '{email}' is invalid.");