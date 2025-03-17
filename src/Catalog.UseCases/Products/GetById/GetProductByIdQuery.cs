namespace Catalog.UseCases.Products.GetById;

public sealed record GetProductByIdQuery(Guid Id) : IQuery<ProductDto>;
