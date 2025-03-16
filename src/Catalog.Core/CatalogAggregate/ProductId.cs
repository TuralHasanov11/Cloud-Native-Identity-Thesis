namespace Catalog.Core.CatalogAggregate;

public sealed record ProductId(Guid Value)
{
    public static implicit operator Guid(ProductId self) => self.Value;
}
