using MySpot.Application.Abstractions;

namespace MySpot.Application.Commands;

public record SignUpCommand
(
    Guid Id,
    string Email,
    string Username,
    string Password,
    string FullName,
    string Role
) : ICommand;