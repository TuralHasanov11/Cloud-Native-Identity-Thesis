namespace Catalog.UseCases.Products.ListByIds;

public sealed record ListProductsByIdsQuery(Guid[] Ids) : IQuery<IEnumerable<ProductDto>>;
