namespace Catalog.UseCases.Products.ListByIds;

public sealed record GetProductByIdQueryHandler(int[] Ids) : IQuery<IEnumerable<ProductDto>>;
