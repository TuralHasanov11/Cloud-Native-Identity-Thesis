namespace Ordering.Core.CustomerAggregate;

public sealed record IdentityId(string Value)
{
    public static implicit operator string(IdentityId self) => self.Value;
}
