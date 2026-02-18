using MySpot.Application.Abstractions;
using MySpot.Application.Exceptions;
using MySpot.Core.Abstractions;
using MySpot.Core.DomainServices;
using MySpot.Core.Entities;
using MySpot.Core.Exceptions;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Commands.Handlers;

public sealed class ReserveParkingSpotForVehicleCommandHandler(IClock clock,
    IWeeklyParkingSpotRepository weeklyParkingSpotRepository,
    IParkingReservationService parkingReservationService,
    IUserRepository userRepository) : ICommandHandler<ReserveParkingSpotForVehicleCommand>
{
    public async Task HandleAsync(ReserveParkingSpotForVehicleCommand command)
    {
        var weeklyParkingSpots = (await weeklyParkingSpotRepository
                .GetByWeekAsync(new Week(clock.Current())))
            .ToList();
        
        WeeklyParkingSpot? parkingSpotToReserve = weeklyParkingSpots.SingleOrDefault(
            weeklyParkingSpot => weeklyParkingSpot.Id.Value == command.ParkingSpotId);
        
        if (parkingSpotToReserve == null)
        {
            throw new WeeklyParkingSpotNotFoundException((ParkingSpotId)command.ParkingSpotId);
        }
        
        var user = await userRepository.GetByIdAsync(command.UserId);
        if (user is null)
        {
            throw new UserNotFoundException(command.UserId);
        }
        
        var newReservation = new VehicleReservation(command.ReservationId, 
            command.ParkingSpotId,
            command.Capacity,
            new Date(command.Date),
            new Date(clock.Current()),
            new EmployeeName(user.FullName),
            command.LicensePlate
        );
        
        parkingReservationService.ReserveSpotForVehicle(
            weeklyParkingSpots,
            JobTitle.Employee,
            parkingSpotToReserve,
            newReservation);
        await weeklyParkingSpotRepository.UpdateAsync(parkingSpotToReserve);
    }
}