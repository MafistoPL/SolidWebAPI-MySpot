using MySpot.Application.Commands;
using MySpot.Application.DTO;

namespace MySpot.Application.services;

public interface IReservationsService
{
    Task<ReservationDto?> GetAsync(Guid id);
    Task<IEnumerable<ReservationDto>> GetAllWeeklyAsync();
    Task<Guid?> ReserveForVehicleAsync(ReserveParkingSpotForVehicleCommand command);
    Task ReserveForCleaningAsync(ReserveParkingSpotForCleaningCommand command);
    Task<bool> ChangeReservationLicensePlateAsync(ChangeReservationLicensePlateCommand command);
    Task<bool> DeleteAsync(DeleteReservationCommand command);
}
