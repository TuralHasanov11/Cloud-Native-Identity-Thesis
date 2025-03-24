namespace Catalog.UseCases.Brands;
public static partial class BrandExtensions
{
    public static BrandDto ToBrandDto(this Brand product)
    {
        return new BrandDto(
            product.Id,
            product.Name);
    }
}
