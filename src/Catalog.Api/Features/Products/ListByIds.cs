using Catalog.UseCases.Products.ListByIds;

namespace Catalog.Api.Features.Products;

public static class ListByIds
{
    public static async Task<Ok<IEnumerable<ProductDto>>> Handle(
        IMediator mediator,
        int[] ids)
    {
        var result = await mediator.Send(new GetProductByIdQueryHandler(ids));
        return TypedResults.Ok(result.Value);
    }
}
