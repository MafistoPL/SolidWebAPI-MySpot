using MySpot.Application.Abstractions;
using MySpot.Application.Exceptions;
using MySpot.Application.Security;
using MySpot.Core.Repositories;

namespace MySpot.Application.Commands.Handlers;

internal sealed class SignInCommandHandler(
    IUserRepository userRepository,
    IAuthenticator authenticator,
    IPasswordManager passwordManager,
    ITokenStorage tokenStorage
    ) : ICommandHandler<SignInCommand>
{
    public async Task HandleAsync(SignInCommand command)
    {
        var user = await userRepository.GetByEmailAsync(command.Email);
        if (user is null)
        {
            throw new InvalidCredentialsException();
        }

        if (!passwordManager.Validate(command.Password, user.Password))
        {
            throw new InvalidCredentialsException();
        }
        
        var jwt = authenticator.CreateToken(user.Id, user.Role);
        tokenStorage.Set(jwt);
    }
}