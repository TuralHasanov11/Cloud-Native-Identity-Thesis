namespace Catalog.UseCases.ProductTypes.List;

public sealed record ListProductTypesQuery() : IQuery<IEnumerable<ProductTypeDto>>;
