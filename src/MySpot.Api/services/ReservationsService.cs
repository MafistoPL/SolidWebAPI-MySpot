using MySpot.Api.Commands;
using MySpot.Api.DTO;
using MySpot.Api.Entities;
using MySpot.Api.ValueObjects;

namespace MySpot.Api.services;

public class ReservationsService(IEnumerable<WeeklyParkingSpot> weeklyParkingSpots, Clock clock)
{
    public ReservationDto? Get(Guid id) 
        => GetAllWeekly().SingleOrDefault(spot => spot.Id == id);

    public IEnumerable<ReservationDto> GetAllWeekly() 
        => weeklyParkingSpots.SelectMany(spot => spot.Reservations)
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
        var weeklyParkingSpot = weeklyParkingSpots.SingleOrDefault(
            spot =>  spot.Id.Value == createReservationCommand.ParkingSpotId);
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
        => weeklyParkingSpots.SingleOrDefault(spot => spot.Reservations.Any(
            reservation => reservation.Id == reservationId));
}
