using Microsoft.Extensions.DependencyInjection;
using MySpot.Core.DomainServices;
using MySpot.Core.Policies;

namespace MySpot.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddSingleton<IReservationPolicy, BossReservationPolicy>();
        services.AddSingleton<IReservationPolicy, ManagerReservationPolicy>();
        services.AddSingleton<IReservationPolicy, RegularEmployeeReservationPolicy>();
        
        services.AddSingleton<IParkingReservationService, ParkingReservationService>();
        
        return services;
    }
}