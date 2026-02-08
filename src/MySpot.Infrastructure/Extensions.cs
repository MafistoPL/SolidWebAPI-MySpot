using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.services;
using MySpot.Core.Repositories;
using MySpot.Infrastructure.Repository;

namespace MySpot.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddSingleton<IClock, Clock>()
            .AddSingleton<IWeeklyParkingSpotRepository, InMemoryWeeklyParkingSpotRepository>();
        
        return services;
    }
}