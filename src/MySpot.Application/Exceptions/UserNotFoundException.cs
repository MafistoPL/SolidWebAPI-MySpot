using MySpot.Core.Exceptions;

namespace MySpot.Application.Exceptions;

public sealed class UserNotFoundException(Guid message) : MySpotException($"User with id: {message} not found.");
