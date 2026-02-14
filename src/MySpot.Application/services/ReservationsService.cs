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

    private async Task<IEnumerable<ReservationDto>> GetAllWeeklyAsync()
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
}
