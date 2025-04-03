namespace Webhooks.Api.Features.Webhooks;

public record WebhookSubscriptionDto(
    Guid Id,
    string Type,
    DateTime Date,
    Uri DestinationUrl,
    string Token,
    string UserId);
