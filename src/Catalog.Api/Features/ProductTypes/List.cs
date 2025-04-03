namespace Catalog.Api.Features.ProductTypes;

public static class List
{
    public static async Task<Ok<IEnumerable<ProductTypeDto>>> Handle(
        IProductTypeRepository productTypeRepository,
        CancellationToken cancellationToken = default)
    {
        var productTypes = await productTypeRepository.ListAsync(
            new GetProductTypesSpecification(),
            p => p.ToProductTypeDto(),
            cancellationToken);

        return TypedResults.Ok(productTypes);
    }
}
