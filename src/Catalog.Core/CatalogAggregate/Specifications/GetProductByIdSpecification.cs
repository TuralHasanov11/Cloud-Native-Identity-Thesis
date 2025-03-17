namespace Catalog.Core.CatalogAggregate.Specifications;

public class GetProductByIdSpecification(ProductId Id) : Specification<Product>(p => p.Id == Id);
