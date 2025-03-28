namespace Catalog.Api.Features.Products;

public static class List
{
    public static async Task<Ok<PaginatedItems<ProductDto, Guid>>> Handle(
        IProductRepository productRepository,
        string? name,
        Guid? type,
        Guid? brand,
        int pageSize = 10,
        Guid? pageCursor = default,
        CancellationToken cancellationToken = default)
    {
        var specification = new GetProductsSpecification()
            .WithNameCriteria(name)
            .WithProductTypeCriteria(type.HasValue ? new ProductTypeId(type.Value) : null)
            .WithBrandCriteria(brand.HasValue ? new BrandId(brand.Value) : null);

        (var products, long count) = await productRepository.ListAsync(
            specification,
            new ProductId(pageCursor ?? Guid.Empty),
            pageSize,
            cancellationToken);

        return TypedResults.Ok(new PaginatedItems<ProductDto, Guid>(
            pageCursor ?? Guid.Empty,
            pageSize,
            count,
            products.Select(p => p.ToProductDto())));
    }
}
