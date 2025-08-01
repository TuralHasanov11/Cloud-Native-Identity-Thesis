﻿namespace Catalog.Api.Features.Products;

public static class List
{
    public static async Task<Ok<PaginatedItems<ProductDto, Guid>>> Handle(
        IProductRepository productRepository,
        string? name,
        Guid? type,
        Guid? brand,
        int pageSize = 50,
        Guid? pageCursor = default,
        CancellationToken cancellationToken = default)
    {
        var specification = new ProductSpecification()
            .AddNameCriteria(name)
            .AddProductTypeCriteria(type.HasValue ? new ProductTypeId(type.Value) : null)
            .AddBrandCriteria(brand.HasValue ? new BrandId(brand.Value) : null);

        (var products, long count) = await productRepository.ListAsync(
            specification,
            new ProductId(pageCursor ?? Guid.Empty),
            pageSize,
            cancellationToken);

        return TypedResults.Ok(new PaginatedItems<ProductDto, Guid>(
            pageCursor ?? Guid.Empty,
            pageSize,
            count,
            products.Select(p => p.ToProductDto())));
    }
}
