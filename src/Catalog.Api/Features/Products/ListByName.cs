namespace Catalog.Api.Features.Products;

public static class ListByName
{
    public static async Task<Ok<PaginatedItems<ProductDto, Guid>>> Handle(
        IMediator mediator,
        string? name,
        int pageSize = 10,
        Guid? pageCursor = default)
    {
        return await List.Handle(mediator, name, null, null, pageSize, pageCursor);
    }
}
