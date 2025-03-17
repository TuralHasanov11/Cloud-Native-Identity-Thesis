using Catalog.UseCases.Products.Create;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Features.Products;

public static class Create
{
    public static async Task<Results<Created, BadRequest<ProblemDetails>>> Handle(
        IMediator mediator,
        CreateProductRequest request)
    {
        var result = await mediator.Send(
            new CreateProductCommand(
                request.Name,
                request.Description,
                request.Price,
                request.ProductTypeId,
                request.BrandId,
                request.AvailableStock,
                request.RestockThreshold,
                request.MaxStockThreshold));

        return TypedResults.Created(new Uri($"/api/catalog/products/{result.Value.Id}", UriKind.Relative));
    }
}
