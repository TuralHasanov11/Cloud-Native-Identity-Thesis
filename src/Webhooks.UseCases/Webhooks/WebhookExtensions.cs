namespace Webhooks.UseCases.Webhooks;

public static class WebhookExtensions
{
    public static WebhookSubscriptionDto ToWebhookSubscriptionDto(this WebhookSubscription webhook)
    {
        return new WebhookSubscriptionDto(webhook.Id);
    }
}
