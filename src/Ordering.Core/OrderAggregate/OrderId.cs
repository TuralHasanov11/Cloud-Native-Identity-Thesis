namespace Ordering.Core.OrderAggregate;

public sealed record OrderId(Guid Value)
{
    public static implicit operator Guid(OrderId orderId) => orderId.Value;
}
