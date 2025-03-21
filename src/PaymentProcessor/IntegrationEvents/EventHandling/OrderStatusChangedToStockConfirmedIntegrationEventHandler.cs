using MassTransit;
using Microsoft.Extensions.Options;

namespace PaymentProcessor.IntegrationEvents.EventHandling;

public class OrderStatusChangedToStockConfirmedIntegrationEventHandler(
    IPublishEndpoint eventBus,
    IOptionsMonitor<PaymentOptions> options,
    ILogger<OrderStatusChangedToStockConfirmedIntegrationEventHandler> logger) :
    IConsumer<OrderStatusChangedToStockConfirmedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderStatusChangedToStockConfirmedIntegrationEvent> context)
    {
        logger.LogHandlingIntegrationEvent(context.Message.Id, context.Message);

        IntegrationEvent orderPaymentIntegrationEvent;

        if (options.CurrentValue.PaymentSucceeded)
        {
            orderPaymentIntegrationEvent = new OrderPaymentSucceededIntegrationEvent(context.Message.OrderId);
        }
        else
        {
            orderPaymentIntegrationEvent = new OrderPaymentFailedIntegrationEvent(context.Message.OrderId);
        }

        logger.LogPublishingIntegrationEvent(orderPaymentIntegrationEvent.Id, orderPaymentIntegrationEvent);

        await eventBus.Publish(orderPaymentIntegrationEvent);
    }
}


public static partial class OrderStatusChangedToStockConfirmedIntegrationEventHandlerLogger
{
    [LoggerMessage(LogLevel.Information, "Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})")]
    public static partial void LogHandlingIntegrationEvent(
        this ILogger<OrderStatusChangedToStockConfirmedIntegrationEventHandler> logger,
        Guid integrationEventId,
        OrderStatusChangedToStockConfirmedIntegrationEvent integrationEvent);

    [LoggerMessage(LogLevel.Information, "Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})")]
    public static partial void LogPublishingIntegrationEvent(
        this ILogger<OrderStatusChangedToStockConfirmedIntegrationEventHandler> logger,
        Guid integrationEventId,
        IntegrationEvent integrationEvent);
}
