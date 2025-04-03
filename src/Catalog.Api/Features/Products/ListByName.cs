namespace Catalog.Api.Features.Products;

public static class ListByName
{
    public static async Task<Ok<PaginatedItems<ProductDto, Guid>>> Handle(
        IProductRepository productRepository,
        string? name,
        int pageSize = 10,
        Guid? pageCursor = default,
        CancellationToken cancellationToken = default)
    {
        return await List.Handle(
            productRepository,
            name,
            null,
            null,
            pageSize,
            pageCursor,
            cancellationToken);
    }
}
