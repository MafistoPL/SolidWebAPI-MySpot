using MySpot.Application;
using MySpot.Core;
using MySpot.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddCore()
    .AddControllers();

var app = builder.Build();

app.UseInfrastructure();
app.Run();

namespace MySpot.Api
{
    public partial class Program
    {
    }
}
