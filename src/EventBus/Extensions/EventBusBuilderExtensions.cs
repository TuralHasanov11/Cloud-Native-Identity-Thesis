namespace EventBus.Extensions;

public static class EventBusBuilderExtensions
{
    //public static void ConfigureEventBus(this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies)
    //{
    //    services.AddOptions<MessageBrokerSettings>()
    //        .Bind(configuration.GetSection(MessageBrokerSettings.SectionName))
    //        .ValidateDataAnnotations()
    //        .ValidateOnStart();

    //    services.AddSingleton<IValidateOptions<MessageBrokerSettings>, ValidateMessageBrokerSettings>();

    //    services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

    //    services.AddMassTransit(options =>
    //    {
    //        options.SetKebabCaseEndpointNameFormatter();

    //        options.AddConsumers(assemblies);

    //        options.UsingRabbitMq((context, config) =>
    //        {
    //            var settings = context.GetRequiredService<MessageBrokerSettings>();

    //            config.Host(new Uri(settings.Host), h =>
    //            {
    //                h.Username(settings.Username);
    //                h.Password(settings.Password);
    //            });

    //            config.ConfigureEndpoints(context);
    //        });
    //    });
    //}
}
