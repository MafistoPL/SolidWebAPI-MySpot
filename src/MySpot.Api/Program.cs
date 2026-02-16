using MySpot.Application;
using MySpot.Core;
using MySpot.Infrastructure;
using MySpot.Infrastructure.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddCore()
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddControllers();

builder.UseSerilog();

var app = builder.Build();

app.UseInfrastructure();
app.Run();

namespace MySpot.Api
{
    public partial class Program
    {
    }
}
