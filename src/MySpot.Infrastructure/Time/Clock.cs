namespace MySpot.Application.services;

public class Clock : IClock
{
    public DateTime Current() => DateTime.UtcNow;
}
