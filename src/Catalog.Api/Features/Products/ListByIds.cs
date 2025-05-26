namespace Catalog.Api.Features.Products;

public static class ListByIds
{
    public static async Task<Ok<IEnumerable<ProductDto>>> Handle(
        IProductRepository productRepository,
        Guid[] ids,
        CancellationToken cancellationToken)
    {
        var specification = new GetProductsSpecification([.. ids.Select(i => new ProductId(i))]);

        var products = await productRepository.ListAsync(
            specification,
            p => p.ToProductDto(),
            cancellationToken);

        return TypedResults.Ok(products);
    }
}
