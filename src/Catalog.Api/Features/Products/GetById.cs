using Catalog.UseCases.Products.GetById;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Catalog.Api.Features.Products;

public static class GetById
{
    public static async Task<Results<Ok<ProductDto>, NotFound>> Handle(
        IMediator mediator,
        Guid id)
    {
        var result = await mediator.Send(new GetProductByIdQuery(id));

        if (result.IsNotFound())
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(result.Value);
    }
}
