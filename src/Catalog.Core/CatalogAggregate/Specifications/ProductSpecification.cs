using Microsoft.EntityFrameworkCore;

namespace Catalog.Core.CatalogAggregate.Specifications;

public class ProductSpecification : Specification<Product>
{
    public ProductSpecification()
        : base()
    {
    }

    public ProductSpecification(ProductId Id)
        : base(p => p.Id == Id)
    {
    }

    public ProductSpecification(IEnumerable<ProductId> Ids)
        : base(p => Ids.Contains(p.Id))
    {

    }

    public ProductSpecification WithBrand()
    {
        AddInclude(query => query.Include(ci => ci.Brand));

        return this;
    }


    public ProductSpecification AddNameCriteria(string? name)
    {
        AddCriteria(p => p.Name == name, !string.IsNullOrEmpty(name));
        return this;
    }

    public ProductSpecification AddProductTypeCriteria(ProductTypeId? productTypeId = null)
    {
        AddCriteria(p => p.ProductTypeId == productTypeId, productTypeId != null);
        return this;
    }

    public ProductSpecification AddBrandCriteria(BrandId? brandId = null)
    {
        AddCriteria(p => p.BrandId == brandId, brandId != null);
        return this;
    }
}
