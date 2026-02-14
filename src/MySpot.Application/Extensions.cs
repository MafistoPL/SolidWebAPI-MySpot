using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.Abstractions;
using MySpot.Application.Commands;
using MySpot.Application.Commands.Handlers;
using MySpot.Application.services;

namespace MySpot.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IReservationsService, ReservationsService>();
        services
            .AddScoped<ICommandHandler<ReserveParkingSpotForVehicleCommand>,
                ReserveParkingSpotForVehicleCommandHandler>();
        services
            .AddScoped<ICommandHandler<ChangeReservationLicensePlateCommand>,
                ChangeReservationLicensePlateCommandHandler>();
        
        return services;
    }
}
