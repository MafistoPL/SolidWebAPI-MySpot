using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySpot.Core.Repositories;
using MySpot.Infrastructure.DAL.Repository;

namespace MySpot.Infrastructure.DAL;

internal static class Extensions
{
    public static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgres");
        
        services.AddDbContext<MySpotDbContext>(
            x => x.UseNpgsql(connectionString));
        services.AddScoped<IWeeklyParkingSpotRepository, EfCoreWeeklyParkingSpotRepository>();
        services.AddScoped<IReservationRepository, EfCoreReservationRepository>();
        services.AddHostedService<DatabaseInitializer>();
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
        return services;
    }
}
