using Catalog.Core.CatalogAggregate;

namespace Catalog.UseCases.Brands;
public static partial class BrandMapper
{
    public static BrandDto ToBrandDto(this Brand product)
    {
        return new BrandDto(
            product.Id,
            product.Name);
    }
}
