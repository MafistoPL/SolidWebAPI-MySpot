using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.services;

public class ReservationsService(
    IWeeklyParkingSpotRepository weeklyParkingSpotRepository,
    IClock clock) 
    : IReservationsService
{
    public ReservationDto? Get(Guid id) 
        => GetAllWeekly().SingleOrDefault(spot => spot.Id == id);

    public IEnumerable<ReservationDto> GetAllWeekly() 
        => weeklyParkingSpotRepository.GetAll().SelectMany(spot => spot.Reservations)
            .Select(reservation => new ReservationDto
            {
                Id = reservation.Id,
                ParkingSpotId = reservation.ParkingSpotId,
                EmployeeName = reservation.EmployeeName,
                LicensePlate = reservation.LicensePlate,
                Date = reservation.Date.Value.Date
            });

    public Guid? Create(CreateReservationCommand createReservationCommand)
    {
        var weeklyParkingSpot = weeklyParkingSpotRepository.Get(
            createReservationCommand.ParkingSpotId);
        
        if (weeklyParkingSpot == null)
        {
            return null;
        }

        var newReservation = new Reservation(createReservationCommand.ReservationId, 
            createReservationCommand.ParkingSpotId,
            createReservationCommand.EmployeeName,
            createReservationCommand.LicensePlate,
            new Date(createReservationCommand.Date),
            new Date(clock.Current()));
        
        weeklyParkingSpot.AddReservation(newReservation, new Date(clock.Current()));

        return newReservation.Id;
    }

    public bool Update(ChangeReservationLicensePlateCommand command)
    {
        var weeklyParkingSpot = GetWeeklyParkingSpotByReservation(command.ReservationId);
        if (weeklyParkingSpot == null)
        {
            return false;
        }
        
        var existingReservation = weeklyParkingSpot.Reservations.SingleOrDefault(
            reservation => reservation.Id == command.ReservationId);
        if (existingReservation == null)
        {
            return false;
        }

        existingReservation.ChangeLicensePlate(command.LicensePlate);

        return true;
    }
    
    public bool Delete(DeleteReservationCommand command)
    {
        var weeklyParkingSpot = GetWeeklyParkingSpotByReservation(command.ReservationId);

        var existingReservation = weeklyParkingSpot?.Reservations.SingleOrDefault(
            reservation => reservation.Id == command.ReservationId);
        if (existingReservation == null)
        {
            return false;
        }
        
        weeklyParkingSpot?.RemoveReservation(existingReservation.Id);
        
        return true;
    }
    
    private WeeklyParkingSpot? GetWeeklyParkingSpotByReservation(Guid reservationId)
        => weeklyParkingSpotRepository.GetAll().SingleOrDefault(spot => spot.Reservations.Any(
            reservation => reservation.Id == reservationId));
}
