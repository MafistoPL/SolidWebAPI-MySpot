using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.services;

namespace MySpot.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IReservationsService, ReservationsService>();
        
        return services;
    }
}
