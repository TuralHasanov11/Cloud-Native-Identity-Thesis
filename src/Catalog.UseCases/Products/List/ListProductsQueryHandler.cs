namespace Catalog.UseCases.Products.List;

public sealed class ListProductsQueryHandler(IProductRepository productRepository)
    : IQueryHandler<ListProductsQuery, PaginatedItems<ProductDto, Guid>>
{
    public async Task<Result<PaginatedItems<ProductDto, Guid>>> Handle(
        ListProductsQuery request,
        CancellationToken cancellationToken)
    {
        var specification = new GetProductsSpecification()
            .WithNameCriteria(request.Name)
            .WithProductTypeCriteria(request.ProductType.HasValue ? new ProductTypeId(request.ProductType.Value) : null)
            .WithBrandCriteria(request.Brand.HasValue ? new BrandId(request.Brand.Value) : null);

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
