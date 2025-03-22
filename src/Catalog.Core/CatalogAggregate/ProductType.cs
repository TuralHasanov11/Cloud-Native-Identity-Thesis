namespace Catalog.Core.CatalogAggregate;

public sealed class ProductType : EntityBase<ProductTypeId>
{
    private ProductType(string name)
    {
        Name = name;
    }

    public string Name { get; private set; }

    public void UpdateName(string name)
    {
        Name = name;
    }

    public static ProductType Create(string name)
    {
        return new ProductType(name);
    }
}
