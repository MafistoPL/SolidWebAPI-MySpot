using MySpot.Application.Abstractions;
using MySpot.Application.Exceptions;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Commands.Handlers;

public class ChangeReservationLicensePlateCommandHandler(IWeeklyParkingSpotRepository weeklyParkingSpotRepository)
    : ICommandHandler<ChangeReservationLicensePlateCommand>
{
    public async Task HandleAsync(ChangeReservationLicensePlateCommand command)
    {
        var weeklyParkingSpot = await GetWeeklyParkingSpotByReservation(command.ReservationId);
        if (weeklyParkingSpot == null)
        {
            throw new WeeklyParkingSpotNotFound((ReservationId)command.ReservationId);
        }
        
        var existingReservation = weeklyParkingSpot.Reservations.
            OfType<VehicleReservation>().
            SingleOrDefault(
                reservation => reservation.Id.Value == command.ReservationId);
        if (existingReservation == null)
        {
            throw new ReservationNotFound(command.ReservationId);
        }

        existingReservation.ChangeLicensePlate(command.LicensePlate);
        await weeklyParkingSpotRepository.UpdateAsync(weeklyParkingSpot);
    }
    
    private async Task<WeeklyParkingSpot?> GetWeeklyParkingSpotByReservation(Guid reservationId)
    {
        var weeklyParkingSpots = await weeklyParkingSpotRepository.GetAllAsync();
        
        return weeklyParkingSpots.SingleOrDefault(spot =>
            spot.Reservations.Any(reservation => reservation.Id.Value == reservationId));
    }
}