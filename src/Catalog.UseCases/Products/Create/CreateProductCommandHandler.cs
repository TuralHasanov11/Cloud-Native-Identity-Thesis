using Catalog.Core.CatalogAggregate;

namespace Catalog.UseCases.Products.Create;

public sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = Product.Create(
            request.Name,
            request.Description,
            request.Price,
            new ProductTypeId(request.ProductTypeId),
            new BrandId(request.BrandId),
            request.AvailableStock,
            request.RestockThreshold,
            request.MaxStockThreshold);

        //product.Embedding = await services.CatalogAI.GetEmbeddingAsync(item);

        await _productRepository.CreateAsync(product);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(product.ToProductDto());
    }
}
