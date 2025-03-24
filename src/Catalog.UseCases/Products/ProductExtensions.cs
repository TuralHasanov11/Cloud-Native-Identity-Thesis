using Catalog.Core.CatalogAggregate;

namespace Catalog.UseCases.Products;
public static partial class ProductExtensions
{
    public static ProductDto ToProductDto(this Product product)
    {
        return new ProductDto(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.ProductTypeId,
            product.BrandId,
            product.AvailableStock,
            product.RestockThreshold,
            product.MaxStockThreshold);
    }
}
