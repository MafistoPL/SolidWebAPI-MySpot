using MySpot.Api.Commands;
using MySpot.Api.DTO;

namespace MySpot.Api.services;

public interface IReservationsService
{
    ReservationDto? Get(Guid id);
    IEnumerable<ReservationDto> GetAllWeekly();
    Guid? Create(CreateReservationCommand createReservationCommand);
    bool Update(ChangeReservationLicensePlateCommand command);
    bool Delete(DeleteReservationCommand command);
}
