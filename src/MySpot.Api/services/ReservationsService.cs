using MySpot.Api.Commands;
using MySpot.Api.DTO;
using MySpot.Api.Entities;

namespace MySpot.Api.services;

public class ReservationsService
{
    private static Clock _clock = new();
    
    private static readonly List<WeeklyParkingSpot> WeeklyParkingSpots = [
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000001"), _clock.Current(), _clock.Current().AddDays(7), "P1"),
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000002"),_clock.Current(), _clock.Current().AddDays(7), "P2"),
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000003"),_clock.Current(), _clock.Current().AddDays(7), "P3"),
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000004"),_clock.Current(), _clock.Current().AddDays(7), "P4"),
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000005"), _clock.Current(), _clock.Current().AddDays(7), "P5")
    ];

    public ReservationDto? Get(Guid id) 
        => GetAllWeekly().SingleOrDefault(spot => spot.Id == id);

    public IEnumerable<ReservationDto> GetAllWeekly() 
        => WeeklyParkingSpots.SelectMany(spot => spot.Reservations)
            .Select(reservation => new ReservationDto
            {
                Id = reservation.Id,
                ParkingSpotId = reservation.ParkingSpotId,
                EmployeeName = reservation.EmployeeName,
                LicensePlate = reservation.LicensePlate,
                Date = reservation.Date,
            });

    public Guid? Create(CreateReservationCommand createReservationCommand)
    {
        var weeklyParkingSpot = WeeklyParkingSpots.SingleOrDefault(
            spot =>  spot.Id == createReservationCommand.ParkingSpotId);
        if (weeklyParkingSpot == null)
        {
            return null;
        }

        var newReservation = new Reservation(createReservationCommand.ReservationId, 
            createReservationCommand.ParkingSpotId,
            createReservationCommand.EmployeeName,
            createReservationCommand.LicensePlate,
            createReservationCommand.Date);
        
        weeklyParkingSpot.AddReservation(newReservation, _clock.Current());

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
        => WeeklyParkingSpots.SingleOrDefault(spot => spot.Reservations.Any(
            reservation => reservation.Id == reservationId));
}
