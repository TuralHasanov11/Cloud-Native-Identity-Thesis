namespace Webhooks.Infrastructure.Services;

public interface IWebhooksRetriever
{

    Task<IEnumerable<WebhookSubscription>> GetSubscriptionsOfType(WebhookType type);
}
