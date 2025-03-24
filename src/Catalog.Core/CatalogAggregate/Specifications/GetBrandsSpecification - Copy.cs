namespace Catalog.Core.CatalogAggregate.Specifications;

public class GetBrandSpecification(BrandId Id) : Specification<Brand>(b => b.Id == Id);
