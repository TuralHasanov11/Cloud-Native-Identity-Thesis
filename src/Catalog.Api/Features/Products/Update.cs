using Catalog.UseCases.Products.Update;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Features.Products;

public static class Update
{
    public static async Task<Results<NoContent, BadRequest<ProblemDetails>, NotFound<ProblemDetails>>> Handle(
        IMediator mediator,
        Guid id,
        UpdateProductRequest request)
    {
        var result = await mediator.Send(
            new UpdateProductCommand(
                id,
                request.Name,
                request.Description,
                request.Price,
                request.ProductTypeId,
                request.BrandId,
                request.AvailableStock,
                request.RestockThreshold,
                request.MaxStockThreshold));

        return result.IsNotFound()
            ? TypedResults.NotFound<ProblemDetails>(new()
            {
                Detail = $"Item with id {id} not found."
            })
            : TypedResults.NoContent();
    }
}
