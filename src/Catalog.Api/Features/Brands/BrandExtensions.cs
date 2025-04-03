namespace Catalog.Api.Features.Brands;

public static class BrandExtensions
{
    public static BrandDto ToBrandDto(this Brand brand)
    {
        return new BrandDto(
            brand.Id,
            brand.Name);
    }
}
