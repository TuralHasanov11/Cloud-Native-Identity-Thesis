namespace Catalog.UseCases.Products.Create;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    Guid ProductTypeId,
    Guid BrandId,
    int AvailableStock,
    int RestockThreshold,
    int MaxStockThreshold) : ICommand<ProductDto>;
