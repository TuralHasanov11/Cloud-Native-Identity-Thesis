namespace Catalog.Core.CatalogAggregate;

public sealed record ProductTypeId(Guid Value)
{
    public static implicit operator Guid(ProductTypeId self) => self.Value;
}
