using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.services;
using MySpot.Core.Repositories;
using MySpot.Infrastructure.DAL;
using MySpot.Infrastructure.DAL.Repository;
using MySpot.Infrastructure.Time;

namespace MySpot.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var sectionAppConfiguration = configuration.GetSection("app");
        services.Configure<AppOptions>(sectionAppConfiguration);
        
        services
            .AddPostgres(configuration)
            .AddScoped<IClock, Clock>();

        return services;
    }
}
