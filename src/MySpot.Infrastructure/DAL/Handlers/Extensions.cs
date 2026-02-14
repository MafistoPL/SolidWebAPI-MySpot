using MySpot.Application.DTO;
using MySpot.Core.Entities;

namespace MySpot.Infrastructure.DAL.Handlers;

internal static class Extensions
{
    public static WeeklyParkingSpotDto ToDto(this WeeklyParkingSpot entity)
        => new()
        {
            Id = entity.Id.ToString(),
            Name = entity.Name,
            Capacity = (int)entity.Capacity.Value,
            From = entity.Week.From.Value.DateTime,
            To = entity.Week.To.Value.DateTime,
            Reservations = entity.Reservations.Select(r => r.ToDto())
        };
    
    public static ReservationDto ToDto(this Reservation entity)
    {
        var vehicleReservation = entity as VehicleReservation;
                
        return new ReservationDto
        {
            Id = entity.Id,
            ParkingSpotId = entity.ParkingSpotId,
            EmployeeName = vehicleReservation is null
                ? string.Empty
                : vehicleReservation.EmployeeName.Value,
            LicensePlate = vehicleReservation is null
                ? string.Empty
                : vehicleReservation.LicensePlate.Value,
            Date = entity.Date.Value.Date,
            Type = entity.GetType().Name
        };
    }
}