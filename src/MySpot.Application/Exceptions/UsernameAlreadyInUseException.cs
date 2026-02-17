using MySpot.Core.Exceptions;

namespace MySpot.Application.Exceptions;

public sealed class UsernameAlreadyInUseException(string username)
    : MySpotException($"Username: '{username}' is already in use.");