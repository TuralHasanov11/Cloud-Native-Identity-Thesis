using System.ComponentModel;
using Catalog.UseCases.Products;
using Catalog.UseCases.Products.ListByIds;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Catalog.Api.Features.Products;

public static class ListByIds
{
    public static async Task<Ok<IEnumerable<ProductDto>>> Handle(
        IMediator mediator,
        [Description("List of ids for catalog items to return")] int[] ids)
    {
        var result = await mediator.Send(new ListProductsByIdsQuery(ids));
        return TypedResults.Ok(result.Value);
    }
}
