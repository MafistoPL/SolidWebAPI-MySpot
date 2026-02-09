using Microsoft.EntityFrameworkCore;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Repository;

public class EfCoreWeeklyParkingSpotRepository(MySpotDbContext context)
    : IWeeklyParkingSpotRepository
{
    public WeeklyParkingSpot? Get(ParkingSpotId id)
        => context.WeeklyParkingSpots
            .Include(spot => spot.Reservations)
            .SingleOrDefault(spot => spot.Id == id);

    public IEnumerable<WeeklyParkingSpot> GetAll()
        => context.WeeklyParkingSpots
            .Include(spot => spot.Reservations)
            .ToList();

    public void Add(WeeklyParkingSpot weeklyParkingSpot)
    {
        context.WeeklyParkingSpots.Add(weeklyParkingSpot);
        context.SaveChanges();
    }

    public void Update(WeeklyParkingSpot weeklyParkingSpot)
    {
        context.WeeklyParkingSpots.Update(weeklyParkingSpot);
        context.SaveChanges();
    }

    public void Remove(WeeklyParkingSpot weeklyParkingSpot)
    {
        context.WeeklyParkingSpots.Remove(weeklyParkingSpot);
        context.SaveChanges();
    }
}
