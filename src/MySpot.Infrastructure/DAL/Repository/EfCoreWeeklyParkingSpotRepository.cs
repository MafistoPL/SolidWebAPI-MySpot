using Microsoft.EntityFrameworkCore;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Repository;

internal class EfCoreWeeklyParkingSpotRepository(MySpotDbContext context)
    : IWeeklyParkingSpotRepository
{
    public Task<WeeklyParkingSpot?> GetAsync(ParkingSpotId id)
        => context.WeeklyParkingSpots
            .Include(spot => spot.Reservations)
            .SingleOrDefaultAsync(spot => spot.Id == id);

    public async Task<IEnumerable<WeeklyParkingSpot>> GetAllAsync()
    {
        var result = await context.WeeklyParkingSpots
            .Include(spot => spot.Reservations)
            .ToListAsync();
        
        return result.AsEnumerable();
    }

    public async Task AddAsync(WeeklyParkingSpot weeklyParkingSpot)
    {
        await context.WeeklyParkingSpots.AddAsync(weeklyParkingSpot);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(WeeklyParkingSpot weeklyParkingSpot)
    {
        context.WeeklyParkingSpots.Update(weeklyParkingSpot);
        await context.SaveChangesAsync();
    }

    public async Task RemoveAsync(WeeklyParkingSpot weeklyParkingSpot)
    {
        context.WeeklyParkingSpots.Remove(weeklyParkingSpot);
        await context.SaveChangesAsync();
    }
}
