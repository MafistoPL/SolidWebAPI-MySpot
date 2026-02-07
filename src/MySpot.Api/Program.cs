using MySpot.Api.Entities;
using MySpot.Api.services;
using MySpot.Api.ValueObjects;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddSingleton<IClock, Clock>()
    .AddSingleton<IEnumerable<WeeklyParkingSpot>>(serviceProvider =>
    {
        var clock = serviceProvider.GetRequiredService<IClock>();
        
        return new List<WeeklyParkingSpot>()
        {
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000001"),
                new Week(clock.Current()), "P1"),
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000002"),
                new Week(clock.Current()), "P2"),
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000003"),
                new Week(clock.Current()), "P3"),
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000004"),
                new Week(clock.Current()), "P4"),
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000005"),
                new Week(clock.Current()), "P5")
        };
    })
    .AddSingleton<ReservationsService>()
    .AddControllers();

var app = builder.Build();
app.MapControllers();
app.Run();

public partial class Program
{
}
