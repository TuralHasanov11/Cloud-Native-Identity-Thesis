namespace Ordering.Core.CustomerAggregate;

public sealed record PaymentMethodId(Guid Value)
{
    public static implicit operator Guid(PaymentMethodId self) => self.Value;
}
