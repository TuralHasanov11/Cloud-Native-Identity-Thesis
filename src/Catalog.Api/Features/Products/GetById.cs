namespace Catalog.Api.Features.Products;

public static class GetById
{
    public static async Task<Results<Ok<ProductDto>, NotFound>> Handle(
        IProductRepository productRepository,
        Guid id,
        CancellationToken cancellationToken)
    {
        var product = await productRepository.SingleOrDefaultAsync(
            new GetProductByIdSpecification(new ProductId(id)).WithBrand(),
            cancellationToken);

        return product == null
            ? (Results<Ok<ProductDto>, NotFound>)TypedResults.NotFound()
            : (Results<Ok<ProductDto>, NotFound>)TypedResults.Ok(product.ToProductDto());
    }
}
