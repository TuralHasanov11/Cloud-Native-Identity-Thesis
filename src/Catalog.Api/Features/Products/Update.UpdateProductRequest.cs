namespace Catalog.Api.Features.Products;

public sealed record UpdateProductRequest(
    string Name,
    string Description,
    decimal Price,
    Guid ProductTypeId,
    Guid BrandId,
    int AvailableStock,
    int RestockThreshold,
    int MaxStockThreshold);
