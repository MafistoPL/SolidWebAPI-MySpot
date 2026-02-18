using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MySpot.Api;
using MySpot.Core.Abstractions;
using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;
using MySpot.Infrastructure.DAL;
using Npgsql;
using Testcontainers.PostgreSql;

namespace MySpot.Tests.Integration.Infrastructure;

public class ApplicationWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public static readonly Guid UserId1 = Guid.Parse("10000000-0000-0000-0000-000000000001");
    public static readonly Guid UserId2 = Guid.Parse("10000000-0000-0000-0000-000000000002");
    public static readonly Guid UserId3 = Guid.Parse("10000000-0000-0000-0000-000000000003");
    public static readonly Guid UserId4 = Guid.Parse("10000000-0000-0000-0000-000000000004");
    public static readonly Guid UserId5 = Guid.Parse("10000000-0000-0000-0000-000000000005");

    static ApplicationWebFactory()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    private static readonly PostgreSqlContainer Container = new PostgreSqlBuilder()
        .WithDatabase("postgres")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private static readonly SemaphoreSlim ContainerLock = new(1, 1);
    private static bool ContainerStarted;
    private static int ContainerUsers;

    private readonly SemaphoreSlim _initializationLock = new(1, 1);
    private bool _initialized;
    private bool _disposed;

    private string? _databaseName;
    private string? _connectionString;

    public TestClock Clock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            if (_connectionString is null)
            {
                throw new InvalidOperationException(
                    "Test database was not initialized. Call InitializeAsync before creating the client.");
            }

            var settings = new Dictionary<string, string?>
            {
                ["ConnectionStrings:Postgres"] = _connectionString
            };
            config.AddInMemoryCollection(settings);
        });

        builder.ConfigureTestServices(services =>
        {
            if (_connectionString is null)
            {
                throw new InvalidOperationException(
                    "Test database was not initialized. Call InitializeAsync before creating the client.");
            }

            services.RemoveAll(typeof(DbContextOptions<MySpotDbContext>));
            services.AddDbContext<MySpotDbContext>(options => options.UseNpgsql(_connectionString));
            services.RemoveAll(typeof(IClock));
            services.AddSingleton<IClock>(Clock);
        });
    }

    public async Task InitializeAsync()
    {
        if (_initialized)
        {
            return;
        }

        await _initializationLock.WaitAsync();
        try
        {
            if (_initialized)
            {
                return;
            }

            await InitializeAsyncCore();
            _initialized = true;
        }
        finally
        {
            _initializationLock.Release();
        }
    }

    public new async Task DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        await DisposeAsyncCore();
    }

    Task IAsyncLifetime.InitializeAsync() => InitializeAsync();

    Task IAsyncLifetime.DisposeAsync() => DisposeAsync();

    private async Task InitializeAsyncCore()
    {
        await EnsureContainerStartedAsync();

        _databaseName = $"myspot_test_{Guid.NewGuid():N}";
        _connectionString = BuildConnectionString(_databaseName);

        await CreateDatabaseAsync(_databaseName);
        await MigrateAndSeedAsync(_connectionString);

        Interlocked.Increment(ref ContainerUsers);
    }

    private async Task DisposeAsyncCore()
    {
        if (_databaseName is not null)
        {
            await DropDatabaseAsync(_databaseName);
        }

        if (Interlocked.Decrement(ref ContainerUsers) == 0)
        {
            await Container.DisposeAsync();
            ContainerStarted = false;
        }
    }

    private static async Task EnsureContainerStartedAsync()
    {
        if (ContainerStarted)
        {
            return;
        }

        await ContainerLock.WaitAsync();
        try
        {
            if (!ContainerStarted)
            {
                await Container.StartAsync();
                ContainerStarted = true;
            }
        }
        finally
        {
            ContainerLock.Release();
        }
    }

    private static string BuildConnectionString(string databaseName)
    {
        var builder = new NpgsqlConnectionStringBuilder(Container.GetConnectionString())
        {
            Database = databaseName
        };

        return builder.ConnectionString;
    }

    private static async Task CreateDatabaseAsync(string databaseName)
    {
        var builder = new NpgsqlConnectionStringBuilder(Container.GetConnectionString())
        {
            Database = "postgres"
        };

        await using var connection = new NpgsqlConnection(builder.ConnectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand($@"CREATE DATABASE ""{databaseName}""", connection);
        await command.ExecuteNonQueryAsync();
    }

    private static async Task DropDatabaseAsync(string databaseName)
    {
        var builder = new NpgsqlConnectionStringBuilder(Container.GetConnectionString())
        {
            Database = "postgres"
        };

        await using var connection = new NpgsqlConnection(builder.ConnectionString);
        await connection.OpenAsync();

        await using (var terminateCommand = new NpgsqlCommand(
                         @"SELECT pg_terminate_backend(pid)
                           FROM pg_stat_activity
                           WHERE datname = @db AND pid <> pg_backend_pid();", connection))
        {
            terminateCommand.Parameters.AddWithValue("db", databaseName);
            await terminateCommand.ExecuteNonQueryAsync();
        }

        await using var dropCommand = new NpgsqlCommand($@"DROP DATABASE IF EXISTS ""{databaseName}""", connection);
        await dropCommand.ExecuteNonQueryAsync();
    }

    private async Task MigrateAndSeedAsync(string connectionString)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        var options = new DbContextOptionsBuilder<MySpotDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        await using var context = new MySpotDbContext(options);
        await context.Database.MigrateAsync();

        var seedParkingSpots = !context.WeeklyParkingSpots.Any();
        var seedUsers = !context.Users.Any();

        if (!seedParkingSpots && !seedUsers)
        {
            return;
        }

        if (seedParkingSpots)
        {
            var week = new Week(new DateTimeOffset(Clock.Current()));
            context.WeeklyParkingSpots.AddRange(
                WeeklyParkingSpot.Create(Guid.Parse("00000000-0000-0000-0000-000000000001"), week, "P1"),
                WeeklyParkingSpot.Create(Guid.Parse("00000000-0000-0000-0000-000000000002"), week, "P2"),
                WeeklyParkingSpot.Create(Guid.Parse("00000000-0000-0000-0000-000000000003"), week, "P3")
            );
        }

        if (seedUsers)
        {
            var createdAt = Clock.Current();
            context.Users.AddRange(
                new User(UserId1, "employee1@myspot.io", "employee1", "secret123", "Employee 1", "user", createdAt),
                new User(UserId2, "employee2@myspot.io", "employee2", "secret123", "Employee 2", "user", createdAt),
                new User(UserId3, "employee3@myspot.io", "employee3", "secret123", "Employee 3", "user", createdAt),
                new User(UserId4, "employee4@myspot.io", "employee4", "secret123", "Employee 4", "user", createdAt),
                new User(UserId5, "employee5@myspot.io", "employee5", "secret123", "Employee 5", "user", createdAt)
            );
        }

        await context.SaveChangesAsync();
    }
}
