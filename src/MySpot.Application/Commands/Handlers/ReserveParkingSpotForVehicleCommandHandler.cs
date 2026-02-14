using MySpot.Application.Abstractions;
using MySpot.Application.Exceptions;
using MySpot.Core.Abstractions;
using MySpot.Core.DomainServices;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Commands.Handlers;

public sealed class ReserveParkingSpotForVehicleCommandHandler(
    IWeeklyParkingSpotRepository weeklyParkingSpotRepository,
    IParkingReservationService parkingReservationService,
    IClock clock) : ICommandHandler<ReserveParkingSpotForVehicleCommand>
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
            throw new WeeklyParkingSpotNotFound((ParkingSpotId)command.ParkingSpotId);
        }

        var newReservation = new VehicleReservation(command.ReservationId, 
            command.ParkingSpotId,
            command.Capacity,
            new Date(command.Date),
            new Date(clock.Current()),
            command.EmployeeName,
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