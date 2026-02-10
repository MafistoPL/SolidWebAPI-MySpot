using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;
using MySpot.Infrastructure.Time;

namespace MySpot.Infrastructure.DAL;

internal sealed class DatabaseInitializer(IServiceScopeFactory scopeFactory) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MySpotDbContext>();
        await dbContext.Database.MigrateAsync(cancellationToken);
    
        var weeklyParkingSpots = await dbContext.WeeklyParkingSpots.ToListAsync(cancellationToken);
        if (weeklyParkingSpots.Any())
        {
            return;
        }
        
        var clock = new Clock();
        weeklyParkingSpots = new List<WeeklyParkingSpot>()
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
        
        dbContext.WeeklyParkingSpots.AddRange(weeklyParkingSpots);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}
