namespace Catalog.Core.CatalogAggregate;

public sealed class ProductType : EntityBase<ProductTypeId>
{
    private ProductType(string name)
        : base(new ProductTypeId(Guid.CreateVersion7()))
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("The name cannot be empty.", nameof(name));
        }

        Name = name;
    }

    public string Name { get; private set; }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("The name cannot be empty.", nameof(name));
        }

        Name = name;
    }

    public static ProductType Create(string name)
    {
        return new ProductType(name);
    }
}
