namespace Catalog.UseCases.Products;

public record ProductDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    Guid ProductTypeId,
    Guid BrandId,
    int availableStock,
    int restockThreshold,
    int maxStockThreshold);


