namespace Catalog.UseCases.Products;

public record ProductDto(
    string Name,
    string Description,
    decimal Price,
    Guid ProductTypeId,
    Guid BrandId,
    int availableStock,
    int restockThreshold,
    int maxStockThreshold);


