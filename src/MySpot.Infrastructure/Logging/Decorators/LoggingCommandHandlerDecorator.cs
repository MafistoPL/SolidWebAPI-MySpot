using System.Diagnostics;
using Humanizer;
using Microsoft.Extensions.Logging;
using MySpot.Application.Abstractions;

namespace MySpot.Infrastructure.Logging.Decorators;

internal sealed class LoggingCommandHandlerDecorator<TCommand>(
    ICommandHandler<TCommand> commandHandler,
    ILogger<ICommandHandler<TCommand>> logger
    ) : ICommandHandler<TCommand>
    where TCommand : class, ICommand
{
    public async Task HandleAsync(TCommand command)
    {
        var commandName = command.GetType().Name.Underscore();
        var stopwatch = new Stopwatch();
        
        stopwatch.Start();
        logger.LogInformation("Started handling command: {CommandName}", commandName);
        
        await commandHandler.HandleAsync(command);
        
        stopwatch.Stop();
        logger.LogInformation("Finished handling command: {CommandName} in {Elapsed}  ms", 
            commandName, stopwatch.ElapsedMilliseconds);
    }
}