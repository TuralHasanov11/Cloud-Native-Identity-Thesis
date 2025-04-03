namespace Catalog.Core.CatalogAggregate;

public sealed class Brand : EntityBase<BrandId>
{
    private Brand(string name)
        : base(new BrandId(Guid.CreateVersion7()))
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("The brand name cannot be null or empty.", nameof(name));
        }

        Name = name;
    }

    public string Name { get; private set; }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("The brand name cannot be null or empty.", nameof(name));
        }

        Name = name;
    }

    public static Brand Create(string name)
    {
        return new Brand(name);
    }
}
