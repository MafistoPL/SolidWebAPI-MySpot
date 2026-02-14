using Microsoft.EntityFrameworkCore;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Repository;

internal class EfCoreReservationRepository(MySpotDbContext context)
    : IReservationRepository
{
    public Task<Reservation?> GetAsync(ReservationId id)
    {
        return context.Reservations.SingleOrDefaultAsync(reservation => reservation.Id == id);
    }

    public async Task<IEnumerable<Reservation>> GetAllAsync()
    {
        var result = await context.Reservations.ToListAsync();
        return result.AsEnumerable();
    }

    public async Task AddAsync(Reservation reservation)
    {
        context.Reservations.Add(reservation);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Reservation reservation)
    {
        context.Reservations.Update(reservation);
        await context.SaveChangesAsync();
    }

    public async Task RemoveAsync(Reservation reservation)
    {
        context.Reservations.Remove(reservation);
        await context.SaveChangesAsync();
    }

    public async Task RemoveAsync(IEnumerable<Reservation> reservations)
    {
        context.Reservations.RemoveRange(reservations);
        await context.SaveChangesAsync();
    }
}
