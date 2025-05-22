using Microsoft.EntityFrameworkCore;

namespace Catalog.Core.CatalogAggregate.Specifications;

public class GetProductSpecification(ProductId Id) : Specification<Product>(p => p.Id == Id)
{
    public Specification<Product> WithBrand()
    {
        AddInclude(query => query.Include(ci => ci.Brand));

        return this;
    }
}
