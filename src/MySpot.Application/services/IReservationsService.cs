using MySpot.Application.Commands;
using MySpot.Application.DTO;

namespace MySpot.Application.services;

public interface IReservationsService
{
    ReservationDto? Get(Guid id);
    IEnumerable<ReservationDto> GetAllWeekly();
    Guid? Create(CreateReservationCommand createReservationCommand);
    bool Update(ChangeReservationLicensePlateCommand command);
    bool Delete(DeleteReservationCommand command);
}
