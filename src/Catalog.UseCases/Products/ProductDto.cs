namespace Catalog.UseCases.Products;

public record ProductDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    Guid ProductTypeId,
    Guid BrandId,
    int AvailableStock,
    int RestockThreshold,
    int MaxStockThreshold);


