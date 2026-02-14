using MySpot.Application.Abstractions;

namespace MySpot.Application.Commands;

public record ReserveParkingSpotForCleaningCommand(DateTime Date) : ICommand;