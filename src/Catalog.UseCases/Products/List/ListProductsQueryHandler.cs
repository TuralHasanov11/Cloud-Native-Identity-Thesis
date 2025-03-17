using Catalog.Core.CatalogAggregate;
using Catalog.Core.CatalogAggregate.Specifications;

namespace Catalog.UseCases.Products.List;

public sealed class ListProductsQueryHandler : IQueryHandler<ListProductsQuery, PaginatedItems<ProductDto, Guid>>
{
    private readonly IProductRepository _productRepository;

    public ListProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<PaginatedItems<ProductDto, Guid>>> Handle(
        ListProductsQuery request,
        CancellationToken cancellationToken)
    {
        var specification = new GetProductsSpecification(request.Name, request.ProductType, request.Brand);

        (var products, long count) = await _productRepository.ListAsync(
            specification,
            ProductMapper.ToProductDto,
            new ProductId(request.PageCursor),
            request.PageSize,
            cancellationToken);

        return new PaginatedItems<ProductDto, Guid>(request.PageCursor, request.PageSize, count, products);
    }
}
