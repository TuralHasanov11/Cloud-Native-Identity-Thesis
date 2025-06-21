using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ordering.IntegrationTests;
using Testcontainers.PostgreSql;

[assembly: AssemblyFixture(typeof(OrderingFactory))]

namespace Ordering.IntegrationTests;

public class OrderingFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithDatabase("ordering")
        .WithWaitStrategy(Wait.ForUnixContainer())
        .Build();

    //private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder()
    //    .WithImage("rabbitmq:4.0-management")
    //    .WithUsername("guest")
    //    .WithHostname("rabbitmq")
    //    .WithPassword("guest")
    //    .Build();

    public HttpClient HttpClient { get; private set; } = default!;

    public async ValueTask InitializeAsync()
    {
        await _dbContainer.StartAsync();
        //await _rabbitMqContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<OrderingDbContext>>();

            services.AddDbContextPool<OrderingDbContext>((sp, options) =>
            {
                options.UseNpgsql(
                    _dbContainer.GetConnectionString(),
                    npgsqlOptionsAction => npgsqlOptionsAction.MigrationsHistoryTable(
                        HistoryRepository.DefaultTableName));
            });

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

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
        //await _rabbitMqContainer.StopAsync();
        //await _rabbitMqContainer.DisposeAsync();
        await base.DisposeAsync();
    }
}
