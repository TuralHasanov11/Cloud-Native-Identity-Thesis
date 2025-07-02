namespace Catalog.Core.CatalogAggregate.Specifications;

public class ProductTypeSpecification : Specification<ProductType>
{
    public ProductTypeSpecification()
        : base()
    {
    }

    public ProductTypeSpecification(ProductTypeId Id)
        : base(pt => pt.Id == Id)
    {
    }
}
