using MySpot.Core.Exceptions;

namespace MySpot.Application.Exceptions;

public sealed class EmailAlreadyInUseException(string email) : MySpotException($"Email: '{email}' is already in use.");