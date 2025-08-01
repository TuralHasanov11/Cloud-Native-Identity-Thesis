﻿using Audit;
using EventBus.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Options;
using ServiceDefaults.Identity;
using Webhooks.Api.Features.Webhooks;

namespace Webhooks.Api.Extensions;

internal static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddDefaultAuthentication();

        builder.AddAudit();

        builder.ConfigureEventBus();

        var connectionString = builder.Configuration.GetConnectionString("Database")
            ?? throw new InvalidOperationException("Connection string 'Database' not found.");

        builder.Services.AddDbContextPool<WebhooksDbContext>((sp, options) =>
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

        builder.Services.AddMigration<WebhooksDbContext>();

        builder.Services.AddTransient<IGrantUrlTesterService, GrantUrlTesterService>();
        builder.Services.AddTransient<IWebhooksSender, WebhooksSender>();
        builder.Services.AddScoped<IWebhookSubscriptionRepository, WebhookSubscriptionRepository>();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddTransient<IIdentityService, IdentityService>();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                var origins = builder.Configuration
                    .GetRequiredSection("ClientOrigins")
                    .Get<Dictionary<string, string>>();

                ArgumentNullException.ThrowIfNull(origins);

                policy.WithOrigins([.. origins.Values])
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
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
