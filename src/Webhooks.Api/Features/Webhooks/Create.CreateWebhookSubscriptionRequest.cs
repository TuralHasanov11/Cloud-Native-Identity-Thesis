namespace Webhooks.Api.Features.Webhooks;

public sealed record CreateWebhookSubscriptionRequest(string Url, string Token, string Event, string GrantUrl);
