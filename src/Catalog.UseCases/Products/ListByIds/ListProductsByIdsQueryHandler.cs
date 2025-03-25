namespace Catalog.UseCases.Products.ListByIds;

public sealed class ListProductsByIdsQueryHandler(IProductRepository productRepository)
    : IQueryHandler<ListProductsByIdsQuery, IEnumerable<ProductDto>>
{
    public async Task<Result<IEnumerable<ProductDto>>> Handle(
        ListProductsByIdsQuery request,
        CancellationToken cancellationToken)
    {
        var specification = new GetProductsSpecification(request.Ids.Select(i => new ProductId(i)));

        var products = await productRepository.ListAsync(
            specification,
            p => p.ToProductDto(),
            cancellationToken);

        return Result.Success(products);
    }
}
