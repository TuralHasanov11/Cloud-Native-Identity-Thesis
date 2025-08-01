﻿using Basket.Infrastructure.Repositories;
using EventBus.Extensions;
using MassTransit;
using Microsoft.Extensions.Options;
using ServiceDefaults.Identity;

namespace Basket.Api.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddDefaultAuthentication();


        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("Redis");
        });

        builder.Services.AddDistributedMemoryCache();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddTransient<IIdentityService, IdentityService>();

        builder.Services.AddSingleton<IBasketRepository, BasketRepository>();

        builder.ConfigureEventBus();

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
