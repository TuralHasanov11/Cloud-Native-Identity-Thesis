namespace Catalog.UseCases.Products.Update;

public sealed class DeleteByIdCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
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

        var productEntry = unitOfWork.GetEntry(product);

        var priceEntry = productEntry.Property(i => i.Price);

        if (priceEntry.IsModified)
        {
            var priceChangedEvent = new ProductPriceChangedIntegrationEvent(product.Id, product.Price, priceEntry.OriginalValue);

            await catalogIntegrationEventService.SaveEventAndCatalogContextChangesAsync(priceChangedEvent);

            await catalogIntegrationEventService.PublishThroughEventBusAsync(priceChangedEvent);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(product.ToProductDto());
    }
}
