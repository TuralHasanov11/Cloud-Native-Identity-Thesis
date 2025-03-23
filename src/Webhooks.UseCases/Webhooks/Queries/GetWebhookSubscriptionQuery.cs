using SharedKernel;

namespace Webhooks.UseCases.Webhooks.Queries;

public sealed record GetWebhookSubscriptionQuery(Guid UserId, Guid Id)
    : IQuery<WebhookSubscriptionDto>;
