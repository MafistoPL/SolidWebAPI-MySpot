using MySpot.Api.services;

namespace MySpot.Tests.Unit.Infrastructure;

public class TestClock : IClock
{
    public DateTime CurrentTime { get; set; }

    public DateTime Current() => CurrentTime;
}
