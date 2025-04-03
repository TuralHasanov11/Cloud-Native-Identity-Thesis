namespace Catalog.Core.CatalogAggregate;

public sealed record BrandId(Guid Value)
{
    public static implicit operator Guid(BrandId self) => self.Value;
}
