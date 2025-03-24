namespace Catalog.UseCases.Products.Update;

public sealed class DeleteByIdCommandHandler(
    IProductRepository productRepository,
    ICatalogIntegrationEventService catalogIntegrationEventService)
    : ICommandHandler<UpdateProductCommand, ProductDto>
{
    public async Task<Result<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.SingleOrDefaultAsync(
            new GetProductByIdSpecification(new ProductId(request.Id)),
            cancellationToken);

        if (product == null)
        {
            return Result.NotFound($"Item with id {request.Id} not found.");
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

        return Result.Success(product.ToProductDto());
    }
}
