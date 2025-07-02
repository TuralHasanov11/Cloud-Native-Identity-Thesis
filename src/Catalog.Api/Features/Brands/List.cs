namespace Catalog.Api.Features.Brands;

public static class List
{
    public static async Task<Ok<IEnumerable<BrandDto>>> Handle(
        IBrandRepository brandRepository,
        CancellationToken cancellationToken)
    {
        var brands = await brandRepository.ListAsync(
            new BrandSpecification(),
            p => p.ToBrandDto(),
            cancellationToken);

        return TypedResults.Ok(brands);
    }
}
