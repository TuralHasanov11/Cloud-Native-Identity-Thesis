namespace Catalog.Api.Features.Products;

public static class GetById
{
    public static async Task<Results<Ok<ProductDto>, NotFound>> Handle(
        IProductRepository productRepository,
        Guid id,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var product = await productRepository.SingleOrDefaultAsync(
            new ProductSpecification(new ProductId(id)).WithBrand(),
            cancellationToken);

        if (product == null)
        {
            return TypedResults.NotFound();
        }

        httpContext.WithETag(() => product.RowVersionValue);

        return TypedResults.Ok(product.ToProductDto());
    }
}
