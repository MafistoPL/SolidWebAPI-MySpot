using MySpot.Application.Commands;
using MySpot.Application.DTO;

namespace MySpot.Application.services;

public interface IReservationsService
{
    Task<ReservationDto?> GetAsync(Guid id);
    Task<IEnumerable<ReservationDto>> GetAllWeeklyAsync();
    Task ReserveForCleaningAsync(ReserveParkingSpotForCleaningCommand command);
    Task<bool> DeleteAsync(DeleteReservationCommand command);
}
