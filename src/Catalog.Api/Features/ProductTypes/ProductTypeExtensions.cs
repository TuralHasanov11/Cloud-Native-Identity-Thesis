namespace Catalog.Api.Features.ProductTypes;

public static class ProductTypeExtensions
{
    public static ProductTypeDto ToProductTypeDto(this ProductType productType)
    {
        return new ProductTypeDto(
            productType.Id,
            productType.Name);
    }
}
