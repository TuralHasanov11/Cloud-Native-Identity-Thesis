using Webhooks.Core.WebhookAggregate.Specifications;
using Webhooks.Infrastructure.Services;

namespace Webhooks.Infrastructure.IntegrationEvents;

public class OrderStatusChangedToShippedIntegrationEventHandler(
    IWebhookSubscriptionRepository webhookSubscriptionRepository,
    IWebhooksSender sender,
    ILogger<OrderStatusChangedToShippedIntegrationEventHandler> logger)
    : IConsumer<OrderStatusChangedToShippedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderStatusChangedToShippedIntegrationEvent> context)
    {
        var subscriptions = await webhookSubscriptionRepository.ListAsync(
            new GetWebhookSubscriptionsSpecification(WebhookType.OrderShipped));

        logger.LogInformation("Received OrderStatusChangedToShippedIntegrationEvent and got {SubscriptionCount} subscriptions to process", subscriptions.Count());

        var whook = new WebhookData(WebhookType.OrderShipped, context.Message);

        await sender.SendAll(subscriptions, whook);
    }
}
