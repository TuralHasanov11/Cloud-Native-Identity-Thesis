using SharedKernel;

namespace Webhooks.UseCases.Webhooks.Commands;

public sealed record CreateWebhookSubscriptionCommand(
    string Url,
    string Token,
    string Event,
    string GrantUrl,
    Guid UserId)
    : ICommand<WebhookSubscriptionDto>;
