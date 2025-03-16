namespace Catalog.Core.CatalogAggregate;

public sealed class Brand : EntityBase<BrandId>
{
    private Brand(string name)
    {
        Name = name;
    }

    public string Name { get; private set; }

    public void UpdateName(string name)
    {
        Name = name;
    }

    public static Brand Create(string name)
    {
        return new Brand(name);
    }
}
