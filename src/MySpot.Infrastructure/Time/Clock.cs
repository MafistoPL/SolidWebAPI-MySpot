using MySpot.Application.services;

namespace MySpot.Infrastructure.Time;

public class Clock : IClock
{
    public DateTime Current() => DateTime.UtcNow;
}
