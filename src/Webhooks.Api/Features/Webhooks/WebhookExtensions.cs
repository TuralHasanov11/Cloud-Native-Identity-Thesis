namespace Webhooks.Api.Features.Webhooks;
public static class WebhookExtensions
{
    public static WebhookSubscriptionDto ToWebhookSubscriptionDto(this WebhookSubscription webhook)
    {
        return new WebhookSubscriptionDto(
            webhook.Id,
            webhook.Type.ToString(),
            webhook.Date,
            webhook.DestinationUrl,
            webhook.Token,
            webhook.UserId);
    }
}
