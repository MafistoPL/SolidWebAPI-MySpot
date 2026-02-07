namespace MySpot.Api.services;

public class Clock : IClock
{
    public DateTime Current() => DateTime.UtcNow;
}
