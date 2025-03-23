namespace Webhooks.UseCases.Webhooks.Commands;

public sealed record DeleteWebhookSubscriptionCommand(
    Guid UserId,
    Guid Id)
    : ICommand<Guid>;
