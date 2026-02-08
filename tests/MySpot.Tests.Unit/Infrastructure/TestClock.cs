using MySpot.Application.services;

namespace MySpot.Tests.Unit.Infrastructure;

public class TestClock : IClock
{
    public DateTime CurrentTime { get; set; } = new DateTime(2022, 08, 10);

    public DateTime Current() => CurrentTime;

    public TestClock(DateTime currentTime)
    {
        CurrentTime = currentTime;
    }
}
