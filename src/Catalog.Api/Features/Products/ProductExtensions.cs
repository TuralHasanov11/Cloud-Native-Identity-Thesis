namespace Catalog.Api.Features.Products;
public static class ProductExtensions
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
