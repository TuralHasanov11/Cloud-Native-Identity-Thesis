namespace Ordering.Core.CustomerAggregate;

public sealed record CustomerId
{
    public Guid Value { get; init; }

    public CustomerId(Guid value)
    {
        Value = value;
    }

    public static implicit operator Guid(CustomerId customerId) => customerId.Value;
}
