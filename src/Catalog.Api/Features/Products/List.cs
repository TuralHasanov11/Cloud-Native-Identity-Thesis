using System.ComponentModel;
using Catalog.UseCases.Products;
using Catalog.UseCases.Products.GetById;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Features.Products;

public static class GetById
{
    public static async Task<Results<Ok<ProductDto>, NotFound, BadRequest<ProblemDetails>>> Handle(
        IMediator mediator,
        [Description("The catalog item id")] Guid id)
    {
        var result = await mediator.Send(new GetProductByIdQuery(id));

        return result.IsNotFound() ? TypedResults.NotFound() : TypedResults.Ok(result.Value);
    }
}
