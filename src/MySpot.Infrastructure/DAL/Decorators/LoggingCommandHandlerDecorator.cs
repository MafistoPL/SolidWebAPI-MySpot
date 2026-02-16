using MySpot.Application.Abstractions;

namespace MySpot.Infrastructure.DAL.Decorators;

internal sealed class LoggingCommandHandlerDecorator<TCommand>(
    ICommandHandler<TCommand> commandHandler
    ) : ICommandHandler<TCommand>
    where TCommand : class, ICommand
{
    public async Task HandleAsync(TCommand command)
    {
        Console.WriteLine($"Processing command: {command.GetType().Name}");
        await commandHandler.HandleAsync(command);
    }
}