using Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Catalog.Api.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<CatalogDbContext>();

        builder.AddAudit();

        var connectionString = builder.Configuration.GetConnectionString("Database")
            ?? throw new InvalidOperationException("Connection string 'Database' not found.");

        builder.Services.AddDbContextPool<CatalogDbContext>((sp, options) =>
        {
            options.UseNpgsql(
                connectionString,
                npgsqlOptionsAction => npgsqlOptionsAction.MigrationsHistoryTable(
                    HistoryRepository.DefaultTableName))
                .AddInterceptors(builder.GetAuditInterceptor(sp));

            if (builder.Environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            }
        });

        //builder.AddNpgsqlDbContext<CatalogContext>("catalogdb", configureDbContextOptions: dbContextOptionsBuilder =>
        //{
        //    dbContextOptionsBuilder.UseNpgsql(builder =>
        //    {
        //        builder.UseVector();
        //    });
        //});

        //// REVIEW: This is done for development ease but shouldn't be here in production
        //builder.Services.AddMigration<CatalogContext, CatalogContextSeed>();

        //// Add the integration services that consume the DbContext
        //builder.Services.AddTransient<IIntegrationEventLogService, IntegrationEventLogService<CatalogContext>>();

        //builder.Services.AddTransient<ICatalogIntegrationEventService, CatalogIntegrationEventService>();

        //builder.AddRabbitMqEventBus("eventbus")
        //       .AddSubscription<OrderStatusChangedToAwaitingValidationIntegrationEvent, OrderStatusChangedToAwaitingValidationIntegrationEventHandler>()
        //       .AddSubscription<OrderStatusChangedToPaidIntegrationEvent, OrderStatusChangedToPaidIntegrationEventHandler>();

        builder.Services.AddOptions<CatalogOptions>()
            .BindConfiguration(nameof(CatalogOptions))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        //if (builder.Configuration["OllamaEnabled"] is string ollamaEnabled && bool.Parse(ollamaEnabled))
        //{
        //    builder.AddOllamaApiClient("embedding")
        //        .AddEmbeddingGenerator();
        //}
        //else if (!string.IsNullOrWhiteSpace(builder.Configuration.GetConnectionString("openai")))
        //{
        //    builder.AddOpenAIClientFromConfiguration("openai");
        //    builder.Services.AddEmbeddingGenerator(sp => sp.GetRequiredService<OpenAIClient>().AsEmbeddingGenerator(builder.Configuration["AI:OpenAI:EmbeddingModel"]!))
        //        .UseOpenTelemetry()
        //        .UseLogging();
        //}

        //builder.Services.AddScoped<ICatalogAI, CatalogAI>();
    }
}
