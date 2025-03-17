namespace Catalog.UseCases.Products.List;

public sealed record ListProductsQuery(
    Guid PageCursor,
    int PageSize,
    string Name,
    Guid? ProductType,
    Guid? Brand) : IQuery<PaginatedItems<ProductDto, Guid>>;
