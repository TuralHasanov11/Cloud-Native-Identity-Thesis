namespace Webhooks.Core.WebhookAggregate;

public sealed record IdentityId
{
    public string Value { get; init; }

    public IdentityId(string value)
    {
        Value = value == string.Empty
            ? throw new ArgumentException("The identity ID cannot be empty.", nameof(value))
            : value;
    }


    public static implicit operator string(IdentityId self) => self.Value;
}
