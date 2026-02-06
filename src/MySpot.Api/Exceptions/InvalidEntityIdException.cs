namespace MySpot.Api.Exceptions;

public sealed class InvalidEntityIdException(object id) : MySpotException($"Cannot set: {id}  as entity identifier.");