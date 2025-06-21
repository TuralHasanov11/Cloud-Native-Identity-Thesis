namespace Ordering.Core.CustomerAggregate;

public sealed class CardType : EntityBase, IAggregateRoot
{
    public string Name { get; private set; }

    private CardType(string name)
        : base()
    {
        Name = ValidateName(name);
    }

    public void UpdateName(string name)
    {
        Name = ValidateName(name);
    }

    public static CardType Create(string name)
    {
        return new CardType(name);
    }

    public static string ValidateName(string name)
    {
        return string.IsNullOrWhiteSpace(name) 
            ? throw new ArgumentException("The name cannot be empty.", nameof(name)) 
            : name;
    }
}
