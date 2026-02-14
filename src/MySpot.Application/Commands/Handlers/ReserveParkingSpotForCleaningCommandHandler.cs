using MySpot.Application.Abstractions;
using MySpot.Core.DomainServices;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Commands.Handlers;

public class ReserveParkingSpotForCleaningCommandHandler(
    IWeeklyParkingSpotRepository weeklyParkingSpotRepository,
    IParkingReservationService parkingReservationService,
    IReservationRepository reservationRepository
    ) : ICommandHandler<ReserveParkingSpotForCleaningCommand>
{
    public async Task HandleAsync(ReserveParkingSpotForCleaningCommand command)
    {
        var week = new Week(command.Date);
        var weeklyParkingSpots = (await weeklyParkingSpotRepository
                .GetByWeekAsync(week))
            .ToList();
        
        var reservationsToRemove = parkingReservationService.ReserveParkingForCleaning(
            weeklyParkingSpots, new Date(command.Date));

        await weeklyParkingSpotRepository.UpdateAsync(weeklyParkingSpots);
        await reservationRepository.RemoveAsync(reservationsToRemove);
    }
}