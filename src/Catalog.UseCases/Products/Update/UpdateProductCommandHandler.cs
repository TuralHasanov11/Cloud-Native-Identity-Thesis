using Catalog.Contracts.IntegrationEvents;
using Catalog.Contracts.IntegrationEvents.Events;
using Catalog.Core.CatalogAggregate;
using Catalog.Core.CatalogAggregate.Specifications;

namespace Catalog.UseCases.Products.Update;

public sealed class DeleteByIdCommandHandler : ICommandHandler<UpdateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICatalogIntegrationEventService _catalogIntegrationEventService;


    public DeleteByIdCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        ICatalogIntegrationEventService catalogIntegrationEventService)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _catalogIntegrationEventService = catalogIntegrationEventService;
    }

    public async Task<Result<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.SingleOrDefaultAsync(
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

        var productEntry = _unitOfWork.GetEntry(product);

        var priceEntry = productEntry.Property(i => i.Price);

        if (priceEntry.IsModified)
        {
            var priceChangedEvent = new ProductPriceChangedIntegrationEvent(product.Id, product.Price, priceEntry.OriginalValue);

            await _catalogIntegrationEventService.SaveEventAndCatalogContextChangesAsync(priceChangedEvent);

            await _catalogIntegrationEventService.PublishThroughEventBusAsync(priceChangedEvent);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(product.ToProductDto());
    }
}
