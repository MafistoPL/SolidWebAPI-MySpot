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
            .Select(reservation => new ReservationDto
            {
                Id = reservation.Id,
                ParkingSpotId = reservation.ParkingSpotId,
                EmployeeName = reservation.EmployeeName,
                LicensePlate = reservation.LicensePlate,
                Date = reservation.Date.Value.Date
            });
    }

    public async Task<Guid?> CreateAsync(CreateReservationCommand createReservationCommand)
    {
        var allParkingSpots = (await weeklyParkingSpotRepository
            .GetByWeekAsync(new Week(clock.Current())))
            .ToList();
        
        WeeklyParkingSpot? parkingSpotToReserve = allParkingSpots.SingleOrDefault(
            weeklyParkingSpot => weeklyParkingSpot.Id.Value == createReservationCommand.ParkingSpotId);
        
        if (parkingSpotToReserve == null)
        {
            return null;
        }

        var newReservation = new Reservation(createReservationCommand.ReservationId, 
            createReservationCommand.ParkingSpotId,
            createReservationCommand.EmployeeName,
            createReservationCommand.LicensePlate,
            new Date(createReservationCommand.Date),
            new Date(clock.Current()));
        
        // weeklyParkingSpot.AddReservation(newReservation, new Date(clock.Current()));
        parkingReservationService.ReserveSpotForVehicle(
            allParkingSpots,
            JobTitle.Employee,
            parkingSpotToReserve,
            newReservation);
        await weeklyParkingSpotRepository.UpdateAsync(parkingSpotToReserve);

        return newReservation.Id;
    }

    public async Task<bool> UpdateAsync(ChangeReservationLicensePlateCommand command)
    {
        var weeklyParkingSpot = await GetWeeklyParkingSpotByReservation(command.ReservationId);
        if (weeklyParkingSpot == null)
        {
            return false;
        }
        
        var existingReservation = weeklyParkingSpot.Reservations.SingleOrDefault(
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
