namespace Ordering.Core.CustomerAggregate;

public sealed class CardType : EntityBase
{
    public string Name { get; }

    public CardType(string name)
    {
        Name = !string.IsNullOrWhiteSpace(name) ? name : throw new OrderingDomainException(nameof(name));
    }
}
