using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MySpot.Application.Abstractions;
using MySpot.Application.DTO;
using MySpot.Core.Abstractions;
using MySpot.Infrastructure.Auth;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace MySpot.Infrastructure.Security;

public class Authenticator(
    IOptions<AuthOptions> authOptions,
    IClock clock
    ) : IAuthenticator
{
    public JwtDto CreateToken(Guid userId, string role)
    {
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        
        string issuer = authOptions.Value.Issuer;
        string audience = authOptions.Value.Audience;
        TimeSpan expiry = authOptions.Value.Expiry ?? TimeSpan.FromHours(1);
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.Value.SigningKey)),
            algorithm: SecurityAlgorithms.HmacSha256);

        var now = clock.Current();
        var expires = now.Add(expiry);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
            new Claim(ClaimTypes.Role, role),
        };
        
        var jwt = new JwtSecurityToken(issuer, audience, claims, now, expires, signingCredentials);
        
        return new JwtDto
        {
            AccessToken = jwtSecurityTokenHandler.WriteToken(jwt)
        };
    }
}