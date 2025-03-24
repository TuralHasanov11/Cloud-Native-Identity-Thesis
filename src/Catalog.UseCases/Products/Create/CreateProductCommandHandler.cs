namespace Catalog.UseCases.Products.Create;

public sealed class CreateProductCommandHandler(
    IProductRepository productRepository)
    : ICommandHandler<CreateProductCommand, ProductDto>
{
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

        await productRepository.CreateAsync(product, cancellationToken);

        await productRepository.SaveChangesAsync(cancellationToken);

        return Result.Success(product.ToProductDto());
    }
}
