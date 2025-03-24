namespace Catalog.UseCases.Products.GetById;

public sealed class GetProductByIdQueryHandler(IProductRepository productRepository)
    : IQueryHandler<GetProductByIdQuery, ProductDto>
{
    public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await productRepository.SingleOrDefaultAsync(
            new GetProductByIdSpecification(new ProductId(request.Id)).WithBrand(),
            cancellationToken);

        if (product == null)
        {
            return Result.NotFound($"Item with id {request.Id} not found.");
        }

        return Result.Success(product.ToProductDto());
    }
}
