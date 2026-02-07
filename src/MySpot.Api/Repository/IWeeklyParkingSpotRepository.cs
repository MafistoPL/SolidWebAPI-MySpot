using MySpot.Api.Entities;
using MySpot.Api.ValueObjects;

namespace MySpot.Api.Repository;

public interface IWeeklyParkingSpotRepository
{
    public WeeklyParkingSpot Get(ParkingSpotId id);
    public IEnumerable<WeeklyParkingSpot> GetAll();
    void Add(WeeklyParkingSpot weeklyParkingSpot);
    void Update(WeeklyParkingSpot weeklyParkingSpot);
    void Remove(WeeklyParkingSpot weeklyParkingSpot);
}