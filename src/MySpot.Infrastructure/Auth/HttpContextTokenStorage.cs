using MySpot.Application.DTO;
using MySpot.Application.Security;

namespace MySpot.Infrastructure.Auth;

internal sealed class HttpContextTokenStorage : ITokenStorage
{
    private JwtDto? _jwtDto = null;
    
    public void Set(JwtDto jwt)
    {
        _jwtDto = jwt;
    }

    public JwtDto? Get()
    {
        return _jwtDto;
    }
}