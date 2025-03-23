using Webhooks.Infrastructure.Data;

namespace Webhooks.Infrastructure.Services;

public class WebhooksRetriever(WebhooksDbContext db) : IWebhooksRetriever
{
    public async Task<IEnumerable<WebhookSubscription>> GetSubscriptionsOfType(WebhookType type)
    {
        return await db.Subscriptions.Where(s => s.Type == type).ToListAsync();
    }
}
