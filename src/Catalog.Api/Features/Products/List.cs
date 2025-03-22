using Catalog.UseCases.Products.List;

namespace Catalog.Api.Features.Products;

public static class List
{
    public static async Task<Ok<PaginatedItems<ProductDto, Guid>>> Handle(
    IMediator mediator,
    string? name,
    Guid? type,
    Guid? brand,
    int pageSize = 10,
    Guid? pageCursor = default)
    {
        var result = await mediator.Send(new ListProductsQuery(
            pageCursor ?? Guid.Empty,
            pageSize,
            name,
            type,
            brand));

        return TypedResults.Ok(result.Value);
    }
}
