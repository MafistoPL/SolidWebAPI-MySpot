using MySpot.Core.Exceptions;

namespace MySpot.Application.Exceptions;

public sealed class InvalidCredentialsException() : MySpotException($"Invalid credentials");