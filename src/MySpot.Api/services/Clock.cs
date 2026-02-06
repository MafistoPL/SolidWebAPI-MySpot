namespace MySpot.Api.services;

public class Clock
{
    public DateTime Current() => DateTime.UtcNow;
}