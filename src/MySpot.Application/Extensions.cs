using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.Abstractions;
using MySpot.Application.Commands;
using MySpot.Application.Commands.Handlers;

namespace MySpot.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddScoped<ICommandHandler<ReserveParkingSpotForVehicleCommand>,
                ReserveParkingSpotForVehicleCommandHandler>();
        services
            .AddScoped<ICommandHandler<ChangeReservationLicensePlateCommand>,
                ChangeReservationLicensePlateCommandHandler>();
        
        services
            .AddScoped<ICommandHandler<DeleteReservationCommand>,
                DeleteReservationCommandHandler>();
        
        services
            .AddScoped<ICommandHandler<ReserveParkingSpotForCleaningCommand>,
                ReserveParkingSpotForCleaningCommandHandler>();
        
        return services;
    }
}
