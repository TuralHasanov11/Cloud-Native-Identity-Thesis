using Catalog.IntegrationTests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore.Migrations;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

[assembly: AssemblyFixture(typeof(CatalogFactory))]

namespace Catalog.IntegrationTests;

public class CatalogFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithDatabase("catalog")
        .Build();

    private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder()
        .WithImage("rabbitmq:4.0-management")
        .WithUsername("guest")
        .WithHostname("rabbitmq")
        .WithPassword("guest")
        .Build();

    public async ValueTask InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await _rabbitMqContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
        await _rabbitMqContainer.StopAsync();
        await _rabbitMqContainer.DisposeAsync();
        await base.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<CatalogDbContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<CatalogDbContext>(
                a => a.UseNpgsql(
                    _dbContainer.GetConnectionString(),
                    npgsqlOptionsAction => npgsqlOptionsAction.MigrationsHistoryTable(
                        HistoryRepository.DefaultTableName)));

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
}
