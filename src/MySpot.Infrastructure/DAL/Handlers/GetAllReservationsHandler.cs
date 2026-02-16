using Microsoft.EntityFrameworkCore;
using MySpot.Application.Abstractions;
using MySpot.Application.DTO;
using MySpot.Application.Queries;
using MySpot.Core.Entities;

namespace MySpot.Infrastructure.DAL.Handlers;

internal class GetAllReservationsHandler(MySpotDbContext dbContext)
    : IQueryHandler<GetAllReservations, IEnumerable<ReservationDto>>
{
    public async Task<IEnumerable<ReservationDto>> HandleAsync(GetAllReservations query)
    {
        var weeklyParkingSpots = await dbContext
            .WeeklyParkingSpots
            .Include(spot => spot.Reservations)
            .AsNoTracking()
            .ToListAsync();
        
        return weeklyParkingSpots.SelectMany(spot => spot.Reservations)
            .Select(reservation => reservation.ToDto());
    }
}