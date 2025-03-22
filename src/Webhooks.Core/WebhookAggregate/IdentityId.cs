namespace Webhooks.Core.WebhookAggregate;

public sealed record IdentityId(Guid Value)
{
    public static implicit operator Guid(IdentityId self) => self.Value;
}
