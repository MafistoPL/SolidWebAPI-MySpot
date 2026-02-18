using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MySpot.Application.Abstractions;
using MySpot.Application.Security;
using MySpot.Infrastructure.Security;

namespace MySpot.Infrastructure.Auth;

internal static class Extensions
{
    private const string SectionName = "auth";
    
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var sectionAuthConfiguration = configuration.GetSection(SectionName);
        services.Configure<AuthOptions>(sectionAuthConfiguration);
        
        var options = configuration.GetOptions<AuthOptions>(SectionName);

        services
            .AddSingleton<IAuthenticator, Authenticator>()
            .AddScoped<ITokenStorage, HttpContextTokenStorage>()
            .AddAuthorization(authorization =>
            {
                authorization.AddPolicy("is-admin", policy =>
                {
                    policy.RequireRole("admin");
                });
            })
            .AddAuthentication(configureOptions: authenticationOptions =>
            {
                authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(bearerOptions =>
            {
                bearerOptions.Audience = options.Audience;
                bearerOptions.IncludeErrorDetails = true; // not in prod env
                bearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = options.Issuer,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SigningKey))
                };
            });
        
        return services;
    }
}