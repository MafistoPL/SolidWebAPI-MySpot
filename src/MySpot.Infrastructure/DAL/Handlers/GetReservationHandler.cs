using Microsoft.EntityFrameworkCore;
using MySpot.Application.Abstractions;
using MySpot.Application.DTO;
using MySpot.Application.Queries;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Handlers;

internal sealed class GetReservationHandler(MySpotDbContext dbContext)
    : IQueryHandler<GetReservation, ReservationDto?>
{
    public async Task<ReservationDto?> HandleAsync(GetReservation query)
    {
        var reservationId = (ReservationId)query.Id;
        var reservation = await dbContext.Reservations
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == reservationId);

        return reservation?.ToDto();
    }
}
