namespace Catalog.UseCases.Products.List;

public sealed class ListBrandsQueryHandler(IProductRepository productRepository)
    : IQueryHandler<ListProductsQuery, PaginatedItems<ProductDto, Guid>>
{
    public async Task<Result<PaginatedItems<ProductDto, Guid>>> Handle(
        ListProductsQuery request,
        CancellationToken cancellationToken)
    {
        var specification = new GetProductsSpecification(request.Name, request.ProductType, request.Brand);

        (var products, long count) = await productRepository.ListAsync(
            specification,
            new ProductId(request.PageCursor),
            request.PageSize,
            cancellationToken);

        return new PaginatedItems<ProductDto, Guid>(
            request.PageCursor,
            request.PageSize,
            count,
            products.Select(p => p.ToProductDto()));
    }
}
