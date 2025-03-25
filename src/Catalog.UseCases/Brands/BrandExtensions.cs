namespace Catalog.UseCases.Brands;

public static class BrandExtensions
{
    public static BrandDto ToBrandDto(this Brand product)
    {
        return new BrandDto(
            product.Id,
            product.Name);
    }
}
