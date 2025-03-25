using Catalog.Core.CatalogAggregate;

namespace Catalog.UseCases.ProductTypes;

public static partial class ProductTypeExtensions
{
    public static ProductTypeDto ToProductTypeDto(this ProductType productType)
    {
        return new ProductTypeDto(
            productType.Id,
            productType.Name);
    }
}
