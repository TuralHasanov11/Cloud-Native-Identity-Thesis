namespace Catalog.Core.CatalogAggregate.Specifications;

public class BrandSpecification : Specification<Brand>
{
    public BrandSpecification()
       : base()
    {

    }

    public BrandSpecification(BrandId id)
        : base(b => b.Id == id)
    {

    }
}
