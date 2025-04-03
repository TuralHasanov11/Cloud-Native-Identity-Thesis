namespace Ordering.Core.OrderAggregate;

public sealed record OrderItemId(Guid Value)
{
    public static implicit operator Guid(OrderItemId self) => self.Value;
}
