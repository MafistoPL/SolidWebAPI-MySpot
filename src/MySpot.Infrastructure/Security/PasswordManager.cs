using Microsoft.AspNetCore.Identity;
using MySpot.Application.Security;

namespace MySpot.Infrastructure.Security;

internal sealed class PasswordManager(IPasswordHasher<string> passwordHasher
    ) : IPasswordManager
{
    public string Secure(string password)
    {
        return passwordHasher.HashPassword(user: string.Empty, password);
    }

    public bool Validate(string password, string hashedPassword)
    {
        return passwordHasher.VerifyHashedPassword(
            user: string.Empty, hashedPassword, password) == PasswordVerificationResult.Success;
    }
}