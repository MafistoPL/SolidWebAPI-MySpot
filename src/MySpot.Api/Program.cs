using MySpot.Api.Entities;
using MySpot.Api.Repository;
using MySpot.Api.services;
using MySpot.Api.ValueObjects;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddSingleton<IClock, Clock>()
    .AddSingleton<IWeeklyParkingSpotRepository, InMemoryWeeklyParkingSpotRepository>()
    .AddSingleton<IReservationsService, ReservationsService>()
    .AddControllers();

var app = builder.Build();
app.MapControllers();
app.Run();

public partial class Program
{
}
