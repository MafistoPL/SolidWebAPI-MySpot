using MySpot.Application.DTO;

namespace MySpot.Application.Abstractions;

public interface IAuthenticator
{
    JwtDto CreateToken(Guid userId, string role);
}