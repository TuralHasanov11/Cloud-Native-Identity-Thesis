using Webhooks.Infrastructure.Services;
using Webhooks.UseCases.Webhooks;

namespace Webhooks.Infrastructure.IntegrationEvents.EventHandlers;

public class OrderStatusChangedToShippedIntegrationEventHandler(
    IWebhooksRetriever retriever,
    IWebhooksSender sender,
    ILogger<OrderStatusChangedToShippedIntegrationEventHandler> logger)
    : IConsumer<OrderStatusChangedToShippedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderStatusChangedToShippedIntegrationEvent> context)
    {
        var subscriptions = await retriever.GetSubscriptionsOfType(WebhookType.OrderShipped);

        logger.LogInformation("Received OrderStatusChangedToShippedIntegrationEvent and got {SubscriptionCount} subscriptions to process", subscriptions.Count());

        var whook = new WebhookData(WebhookType.OrderShipped, context.Message);

        await sender.SendAll(subscriptions, whook);
    }
}
