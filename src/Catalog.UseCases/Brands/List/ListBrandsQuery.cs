namespace Catalog.UseCases.Brands.List;

public sealed record ListBrandsQuery() : IQuery<IEnumerable<BrandDto>>;
