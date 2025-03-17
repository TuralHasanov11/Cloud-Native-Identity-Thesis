using Catalog.UseCases.ProductTypes;
using Catalog.UseCases.ProductTypes.List;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Catalog.Api.Features.ProductTypes;

public static class List
{
    public static async Task<Ok<IEnumerable<ProductTypeDto>>> Handle(
        IMediator mediator)
    {
        var result = await mediator.Send(new ListProductTypesQuery());

        return TypedResults.Ok(result.Value);
    }
}
