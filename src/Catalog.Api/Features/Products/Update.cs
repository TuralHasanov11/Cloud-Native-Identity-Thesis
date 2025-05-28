using Catalog.Contracts.Abstractions;
using Catalog.Contracts.IntegrationEvents;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Features.Products;

public static class Update
{
    public static async Task<Results<NoContent, BadRequest<ProblemDetails>, NotFound<ProblemDetails>, ForbidHttpResult, ProblemHttpResult>> Handle(
        IProductRepository productRepository,
        ICatalogIntegrationEventService catalogIntegrationEventService,
        Guid id,
        UpdateProductRequest request,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var product = await productRepository.SingleOrDefaultAsync(
            new GetProductSpecification(new ProductId(id)),
            cancellationToken);

        if (product == null)
        {
            return TypedResults.NotFound<ProblemDetails>(new()
            {
                Detail = $"Item with id {id} not found."
            });
        }

        if (httpContext.PassesConcurrencyCheck(() => product.RowVersionValue))
        {
            return TypedResults.Problem(
                detail: "The product has been modified by another user. Please reload the product and try again.",
                statusCode: StatusCodes.Status412PreconditionFailed);
        }

        var oldPrice = product.Price;

        product.Update(
            request.Name,
            request.Description,
            request.Price,
            new ProductTypeId(request.ProductTypeId),
            new BrandId(request.BrandId),
            request.AvailableStock,
            request.RestockThreshold,
            request.MaxStockThreshold);

        //product.Embedding = await services.CatalogAI.GetEmbeddingAsync(product);

        if (product.Price != oldPrice)
        {
            var priceChangedEvent = new ProductPriceChangedIntegrationEvent(product.Id, product.Price, oldPrice);

            await catalogIntegrationEventService.SaveEventAndCatalogContextChangesAsync(priceChangedEvent);

            await catalogIntegrationEventService.PublishThroughEventBusAsync(priceChangedEvent);
        }

        await productRepository.SaveChangesAsync(cancellationToken);


        return TypedResults.NoContent();
    }
}
