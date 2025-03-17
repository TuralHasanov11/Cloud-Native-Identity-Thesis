namespace Catalog.UseCases.Products.Update;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    Guid ProductTypeId,
    Guid BrandId,
    int AvailableStock,
    int RestockThreshold,
    int MaxStockThreshold) : ICommand<ProductDto>;
