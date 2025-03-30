namespace Ordering.Core.CustomerAggregate;

public sealed class CardType : EntityBase, IAggregateRoot
{
    public string Name { get; }

    private CardType(string name)
        : base()
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("The name cannot be empty.", nameof(name));
        }

        Name = name;
    }

    public static CardType Create(string name)
    {
        return new CardType(name);
    }
}
