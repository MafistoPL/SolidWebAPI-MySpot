using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.Repositories;

public interface IWeeklyParkingSpotRepository
{
    public Task<WeeklyParkingSpot?>GetAsync(ParkingSpotId id);
    public Task<IEnumerable<WeeklyParkingSpot>> GetByWeekAsync(Week week);
    public Task<IEnumerable<WeeklyParkingSpot>> GetAllAsync();
    Task AddAsync(WeeklyParkingSpot weeklyParkingSpot);
    Task UpdateAsync(WeeklyParkingSpot weeklyParkingSpot);
    Task RemoveAsync(WeeklyParkingSpot weeklyParkingSpot);
}
