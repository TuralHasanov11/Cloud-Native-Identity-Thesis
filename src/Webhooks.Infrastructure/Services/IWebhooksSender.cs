using Webhooks.UseCases.Webhooks;

namespace Webhooks.Infrastructure.Services;

public interface IWebhooksSender
{
    Task SendAll(IEnumerable<WebhookSubscription> receivers, WebhookData data);
}
