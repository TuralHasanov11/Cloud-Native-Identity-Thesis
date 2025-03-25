using Catalog.UseCases.Products.ListByIds;

namespace Catalog.Api.Features.Products;

public static class ListByIds
{
    public static async Task<Ok<IEnumerable<ProductDto>>> Handle(
        IMediator mediator,
        Guid[] ids)
    {
        var result = await mediator.Send(new ListProductsByIdsQuery(ids));
        return TypedResults.Ok(result.Value);
    }
}
