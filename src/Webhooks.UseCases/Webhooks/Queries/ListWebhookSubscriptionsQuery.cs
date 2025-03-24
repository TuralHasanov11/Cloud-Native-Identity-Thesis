namespace Webhooks.UseCases.Webhooks.Queries;

public sealed record ListWebhookSubscriptionsQuery(Guid UserId) : IQuery<IEnumerable<WebhookSubscriptionDto>>;
