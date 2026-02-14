using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Core.Abstractions;
using MySpot.Core.DomainServices;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.services;

public sealed class ReservationsService(
    IWeeklyParkingSpotRepository weeklyParkingSpotRepository,
    IReservationRepository reservationRepository,
    IParkingReservationService parkingReservationService,
    IClock clock) 
    : IReservationsService
{
    public async Task<ReservationDto?> GetAsync(Guid id)
    {
        var reservations = await GetAllWeeklyAsync();
        
        return reservations.SingleOrDefault(spot => spot.Id == id);
    }

    public async Task<IEnumerable<ReservationDto>> GetAllWeeklyAsync()
    {
        var weeklyParkingSpots = await weeklyParkingSpotRepository.GetAllAsync();
        
        return weeklyParkingSpots.SelectMany(spot => spot.Reservations)
            .Select(reservation =>
            {
                var vehicleReservation = reservation as VehicleReservation;
                
                return new ReservationDto
                {
                    Id = reservation.Id,
                    ParkingSpotId = reservation.ParkingSpotId,
                    EmployeeName = vehicleReservation is null
                        ? string.Empty
                        : vehicleReservation.EmployeeName.Value,
                    LicensePlate = vehicleReservation is null
                        ? string.Empty
                        : vehicleReservation.LicensePlate.Value,
                    Date = reservation.Date.Value.Date
                };
            });
    }

    public async Task<Guid?> ReserveForVehicleAsync(ReserveParkingSpotForVehicleCommand command)
    {
        var weeklyParkingSpots = (await weeklyParkingSpotRepository
                .GetByWeekAsync(new Week(clock.Current())))
            .ToList();
        
        WeeklyParkingSpot? parkingSpotToReserve = weeklyParkingSpots.SingleOrDefault(
            weeklyParkingSpot => weeklyParkingSpot.Id.Value == command.ParkingSpotId);
        
        if (parkingSpotToReserve == null)
        {
            return null;
        }

        var newReservation = new VehicleReservation(command.ReservationId, 
            command.ParkingSpotId,
            command.Capacity,
            new Date(command.Date),
            new Date(clock.Current()),
            command.EmployeeName,
            command.LicensePlate
            );
        
        // weeklyParkingSpot.AddReservation(newReservation, new Date(clock.Current()));
        parkingReservationService.ReserveSpotForVehicle(
            weeklyParkingSpots,
            JobTitle.Employee,
            parkingSpotToReserve,
            newReservation);
        await weeklyParkingSpotRepository.UpdateAsync(parkingSpotToReserve);

        return newReservation.Id;
    }

    public async Task ReserveForCleaningAsync(ReserveParkingSpotForCleaningCommand command)
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

    public async Task<bool> ChangeReservationLicensePlateAsync(ChangeReservationLicensePlateCommand command)
    {
        var weeklyParkingSpot = await GetWeeklyParkingSpotByReservation(command.ReservationId);
        if (weeklyParkingSpot == null)
        {
            return false;
        }
        
        var existingReservation = weeklyParkingSpot.Reservations.
            OfType<VehicleReservation>().
            SingleOrDefault(
                reservation => reservation.Id.Value == command.ReservationId);
        if (existingReservation == null)
        {
            return false;
        }

        existingReservation.ChangeLicensePlate(command.LicensePlate);
        await weeklyParkingSpotRepository.UpdateAsync(weeklyParkingSpot);

        return true;
    }
    
    public async Task<bool> DeleteAsync(DeleteReservationCommand command)
    {
        var existingReservation = await reservationRepository.GetAsync(command.ReservationId);
        if (existingReservation == null)
        {
            return false;
        }
        
        await reservationRepository.RemoveAsync(existingReservation);
        
        return true;
    }
    
    private async Task<WeeklyParkingSpot?> GetWeeklyParkingSpotByReservation(Guid reservationId)
    {
        var weeklyParkingSpots = await weeklyParkingSpotRepository.GetAllAsync();
        
        return weeklyParkingSpots.SingleOrDefault(spot =>
            spot.Reservations.Any(reservation => reservation.Id.Value == reservationId));
    }
}
