using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Features.Products;

public static class Create
{
    public static async Task<Results<Created, BadRequest<ProblemDetails>, ForbidHttpResult>> Handle(
        IProductRepository productRepository,
        CreateProductRequest request,
        CancellationToken cancellationToken)
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

        await productRepository.CreateAsync(product, cancellationToken);
        await productRepository.SaveChangesAsync(cancellationToken);

        return TypedResults.Created(new Uri($"/api/catalog/products/{product.Id}", UriKind.Relative));
    }
}
