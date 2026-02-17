using MySpot.Application.Abstractions;
using MySpot.Application.Exceptions;
using MySpot.Application.Security;
using MySpot.Core.Abstractions;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Commands.Handlers;

public sealed class SignUpCommandHandler(
    IClock clock, 
    IPasswordManager passwordManager,
    IUserRepository userRepository
    ) : ICommandHandler<SignUpCommand>
{
    public async Task HandleAsync(SignUpCommand command)
    {
        var userId = new UserId(command.Id);
        var email = new Email(command.Email);
        var username = new Username(command.Username);
        var password = new Password(command.Password);
        var fullName = new FullName(command.FullName);
        var role = new Role(command.Role);

        if (await userRepository.GetByEmailAsync(email) is not null)
        {
            throw new EmailAlreadyInUseException(email);
        }

        if (await userRepository.GetByUsernameAsync(username) is not null)
        {
            throw new UsernameAlreadyInUseException(username);
        }
        
        var securedPassword = passwordManager.Secure(command.Password);
        var user = new User(
            command.Id, 
            command.Email, 
            command.Username,
            securedPassword, 
            command.FullName,
            command.Role,
            createdAt: clock.Current());
        
        await userRepository.AddAsync(user);
    }
}