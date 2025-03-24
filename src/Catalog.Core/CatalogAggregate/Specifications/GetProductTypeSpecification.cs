namespace Catalog.Core.CatalogAggregate.Specifications;

public class GetProductTypeSpecification(ProductTypeId Id) : Specification<ProductType>(pt => pt.Id == Id);
