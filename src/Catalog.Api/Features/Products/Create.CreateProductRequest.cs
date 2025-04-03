namespace Catalog.Api.Features.Products;

public sealed record CreateProductRequest(
    string Name,
    string Description,
    decimal Price,
    Guid ProductTypeId,
    Guid BrandId,
    int AvailableStock,
    int RestockThreshold,
    int MaxStockThreshold);
