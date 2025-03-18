using EventBus.Extensions;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Options;
using Ordering.Contracts.IntegrationEvents;
using Ordering.Infrastructure.Data;
using Outbox.Services;

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

        builder.Services.AddTransient<IOutboxService, OutboxService<OrderingDbContext>>();

        //services.AddMigration<OrderingDbContext, OrderingContextSeed>();

        builder.Services.AddTransient<IOrderingIntegrationEventService, OrderingIntegrationEventService>();

        builder.ConfigureEventBus();

        builder.Services.AddHttpContextAccessor();
        //services.AddTransient<IIdentityService, IdentityService>();

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

        builder.Services.AddScoped<IOrderQueries, OrderQueries>();
        builder.Services.AddScoped<IBuyerRepository, BuyerRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<IRequestManager, RequestManager>();
    }

    private static void AddEventBusSubscriptions(this IEventBusBuilder eventBus)
    {
        eventBus.AddSubscription<GracePeriodConfirmedIntegrationEvent, GracePeriodConfirmedIntegrationEventHandler>();
        eventBus.AddSubscription<OrderStockConfirmedIntegrationEvent, OrderStockConfirmedIntegrationEventHandler>();
        eventBus.AddSubscription<OrderStockRejectedIntegrationEvent, OrderStockRejectedIntegrationEventHandler>();
        eventBus.AddSubscription<OrderPaymentFailedIntegrationEvent, OrderPaymentFailedIntegrationEventHandler>();
        eventBus.AddSubscription<OrderPaymentSucceededIntegrationEvent, OrderPaymentSucceededIntegrationEventHandler>();
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
