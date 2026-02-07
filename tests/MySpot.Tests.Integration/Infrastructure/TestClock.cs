using MySpot.Api.services;

namespace MySpot.Tests.Integration.Infrastructure;

public class TestClock : IClock
{
    public DateTime CurrentTime { get; set; } = new DateTime(2022, 08, 10);

    public DateTime Current() => CurrentTime;
}
