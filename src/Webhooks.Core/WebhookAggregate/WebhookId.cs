namespace Webhooks.Core.WebhookAggregate;

public sealed record WebhookId(Guid Value)
{
    public static implicit operator Guid(WebhookId self) => self.Value;
}
