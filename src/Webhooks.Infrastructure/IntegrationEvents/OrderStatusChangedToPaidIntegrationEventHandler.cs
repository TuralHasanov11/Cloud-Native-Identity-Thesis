using Webhooks.Core.WebhookAggregate.Specifications;
using Webhooks.Infrastructure.Services;

namespace Webhooks.Infrastructure.IntegrationEvents;

public class OrderStatusChangedToPaidIntegrationEventHandler(
    IWebhookSubscriptionRepository webhookSubscriptionRepository,
    IWebhooksSender sender,
    ILogger<OrderStatusChangedToPaidIntegrationEventHandler> logger)
    : IConsumer<OrderStatusChangedToPaidIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderStatusChangedToPaidIntegrationEvent> context)
    {
        var subscriptions = await webhookSubscriptionRepository.ListAsync(
            new GetWebhookSubscriptionsSpecification(WebhookType.OrderPaid));

        if (subscriptions is null)
        {
            logger.LogInformation("No subscriptions found for OrderPaid");
            return;
        }

        logger.LogInformation("Received OrderStatusChangedToShippedIntegrationEvent and got {SubscriptionsCount} subscriptions to process", subscriptions.Count());

        var whook = new WebhookData(WebhookType.OrderPaid, context.Message);

        await sender.SendAll(subscriptions, whook);
    }
}
