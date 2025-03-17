namespace Catalog.UseCases.Products.ListByIds;

public sealed record ListProductsByIdsQuery(int[] Ids) : IQuery<IEnumerable<ProductDto>>;
