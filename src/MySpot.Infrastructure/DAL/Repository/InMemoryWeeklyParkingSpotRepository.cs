using MySpot.Application.services;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Repository;

public class InMemoryWeeklyParkingSpotRepository : IWeeklyParkingSpotRepository
{
    private readonly IClock _clock;
    private static List<WeeklyParkingSpot> _weeklyParkingSpots;

    public InMemoryWeeklyParkingSpotRepository(IClock clock)
    {
        _clock = clock;
        // NOTE: This shared list intentionally leaks state between tests; we will refactor to isolate data.
        _weeklyParkingSpots = new List<WeeklyParkingSpot>()
        {
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000001"),
                new Week(clock.Current()), "P1"),
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000002"),
                new Week(clock.Current()), "P2"),
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000003"),
                new Week(clock.Current()), "P3"),
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000004"),
                new Week(clock.Current()), "P4"),
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000005"),
                new Week(clock.Current()), "P5")
        };
    }
    
    public Task<WeeklyParkingSpot?> GetAsync(ParkingSpotId id)
        => Task.FromResult(_weeklyParkingSpots.SingleOrDefault(spot => spot.Id == id));

    public Task<IEnumerable<WeeklyParkingSpot>> GetAllAsync()
        => Task.FromResult(_weeklyParkingSpots.AsEnumerable());

    public Task AddAsync(WeeklyParkingSpot weeklyParkingSpot)
    {
        _weeklyParkingSpots.Add(weeklyParkingSpot);
        
        return Task.CompletedTask;
    } 

    public Task UpdateAsync(WeeklyParkingSpot weeklyParkingSpot)
    {
        return Task.CompletedTask;
    }

    public Task RemoveAsync(WeeklyParkingSpot weeklyParkingSpot)
    {
        _weeklyParkingSpots.Remove(weeklyParkingSpot);
        
        return Task.CompletedTask;
    }
}
