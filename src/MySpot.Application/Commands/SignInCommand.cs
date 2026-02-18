using MySpot.Application.Abstractions;

namespace MySpot.Application.Commands;

public record SignInCommand(string Email, string Password) : ICommand;