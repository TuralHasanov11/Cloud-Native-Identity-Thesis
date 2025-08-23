namespace WebApp.Server.Features.Catalog;

public record Product(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string? PictureUrl,
    Guid BrandId,
    Brand CatalogBrand,
    Guid ProductTypeId,
    ProductType ProductType)
{
    public static IEnumerable<Product> Empty()
    {
        return [];
    }
}

