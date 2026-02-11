using MySpot.Application.services;
using MySpot.Core.Abstractions;

namespace MySpot.Tests.Integration.Infrastructure;

public class TestClock : IClock
{
    public DateTime CurrentTime { get; set; } = new DateTime(2022, 08, 10, 12, 0, 0);

    public DateTime Current() => CurrentTime;
}
