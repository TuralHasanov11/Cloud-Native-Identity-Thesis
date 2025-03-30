using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace Catalog.IntegrationTests;

public class CatalogFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithDatabase("catalog")
        .Build();

    private DbConnection _dbConnection = default!;

#pragma warning disable IDE0052 // Remove unread private members
    private Respawner _respawner = default!;
#pragma warning restore IDE0052 // Remove unread private members

    private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder()
        .WithImage("rabbitmq:4.0-management")
        .WithUsername("guest")
        .WithHostname("rabbitmq")
        .WithPassword("guest")
        .Build();

    public HttpClient HttpClient { get; private set; } = default!;

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        await _rabbitMqContainer.StartAsync();

        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());

        HttpClient = CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });

        //await InitializeRespawner();
    }

    private async Task InitializeRespawner()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(
            _dbConnection,
            new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = ["public"]
            });
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<CatalogDbContext>>();
            services.RemoveAll<CatalogDbContext>();

            services.AddDbContext<CatalogDbContext>(
                a => a.UseNpgsql(_dbContainer.GetConnectionString()),
                ServiceLifetime.Singleton);

            //services.AddMassTransitTestHarness(x =>
            // {
            //     x.AddDelayedMessageScheduler();

            //     x.SetKebabCaseEndpointNameFormatter();

            //     x.AddConsumers(Infrastructure.AssemblyReference.Assembly);

            //     x.UsingRabbitMq((context, cfg) =>
            //     {
            //         cfg.UseDelayedMessageScheduler();

            //         var settings = context.GetRequiredService<MessageBrokerSettings>();

            //         cfg.Host(new Uri(settings.Host), h =>
            //         {
            //             h.Username(settings.Username);
            //             h.Password(settings.Password);
            //         });

            //         cfg.ConfigureEndpoints(context);
            //     });
            // });
        });
    }

    public async Task ResetDatabaseAsync()
    {
        //await _respawner.ResetAsync(_dbConnection);
    }


    public new async Task DisposeAsync()
    {
        await _dbConnection.DisposeAsync();
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
        await _rabbitMqContainer.DisposeAsync();
    }
}
