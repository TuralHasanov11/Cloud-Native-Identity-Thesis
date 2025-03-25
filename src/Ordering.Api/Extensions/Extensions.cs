using EventBus.Extensions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Options;
using Ordering.Contracts.Abstractions;
using Ordering.Core.CustomerAggregate;
using Ordering.Core.OrderAggregate;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.IntegrationEvents;
using Ordering.Infrastructure.Repositories;
using Outbox.Services;
using ServiceDefaults.Identity;

namespace Ordering.Api.Extensions;
public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddDefaultAuthentication();

        builder.AddAudit();

        var connectionString = builder.Configuration.GetConnectionString("Database")
            ?? throw new InvalidOperationException("Connection string 'Database' not found.");

        builder.Services.AddDbContextPool<OrderingDbContext>((sp, options) =>
        {
            options.UseNpgsql(
                connectionString,
                npgsqlOptionsAction => npgsqlOptionsAction.MigrationsHistoryTable(
                    HistoryRepository.DefaultTableName))
                .AddInterceptors(builder.GetAuditInterceptor(sp));

            //builder.UseVector();

            if (builder.Environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            }
        });

        builder.Services.AddTransient<IOutboxService, OutboxService<OrderingDbContext>>(
            sp => new OutboxService<OrderingDbContext>(
                sp.GetRequiredService<OrderingDbContext>(),
                Contracts.AssemblyReference.Assembly));

        //services.AddMigration<OrderingDbContext, OrderingContextSeed>();

        builder.Services.AddTransient<IOrderingIntegrationEventService, OrderingIntegrationEventService>();

        builder.ConfigureEventBus();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddTransient<IIdentityService, IdentityService>();

        // Configure mediatR
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(
                UseCases.AssemblyReference.Assembly,
                Api.AssemblyReference.Assembly);

            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            //cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
            //cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
        });

        builder.Services.AddValidatorsFromAssembly(Api.AssemblyReference.Assembly);

        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<ICardTypeRepository, CardTypeRepository>();
    }

    private static void ConfigureEventBus(this IHostApplicationBuilder builder)
    {
        builder.Services.AddOptions<MessageBrokerSettings>()
            .Bind(builder.Configuration.GetSection(MessageBrokerSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services.AddSingleton<IValidateOptions<MessageBrokerSettings>, ValidateMessageBrokerSettings>();

        builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

        builder.Services.AddMassTransit(options =>
        {
            options.SetKebabCaseEndpointNameFormatter();

            options.AddConsumers(Infrastructure.AssemblyReference.Assembly);

            options.UsingRabbitMq((context, config) =>
            {
                var settings = context.GetRequiredService<MessageBrokerSettings>();

                config.Host(new Uri(settings.Host), h =>
                {
                    h.Username(settings.Username);
                    h.Password(settings.Password);
                });

                config.ConfigureEndpoints(context);
            });
        });
    }
}
