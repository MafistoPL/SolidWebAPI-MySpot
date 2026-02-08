using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MySpot.Api;
using MySpot.Application.services;

namespace MySpot.Tests.Integration.Infrastructure;

public class ApplicationWebFactory : WebApplicationFactory<Program>
{
    public TestClock Clock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(IClock));
            services.AddSingleton<IClock>(Clock);
        });
    }
}
