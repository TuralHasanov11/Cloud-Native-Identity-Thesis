namespace Webhooks.Core.WebhookAggregate;

public sealed record IdentityId
{
    public Guid Value { get; init; }

    public IdentityId(Guid value)
    {
        Value = value == Guid.Empty
            ? throw new ArgumentException("The identity ID cannot be empty.", nameof(value))
            : value;
    }


    public static implicit operator Guid(IdentityId self) => self.Value;
}
