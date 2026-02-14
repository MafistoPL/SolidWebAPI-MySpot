using MySpot.Application.Abstractions;
using MySpot.Application.DTO;

namespace MySpot.Application.Queries;

public class GetReservation : IQuery<ReservationDto?>
{
    public Guid Id { get; set; }
}
