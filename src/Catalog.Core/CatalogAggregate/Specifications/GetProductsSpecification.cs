namespace Catalog.Core.CatalogAggregate.Specifications;

public class GetProductsSpecification : Specification<Product>
{
    public GetProductsSpecification()
    {

    }

    public GetProductsSpecification(IEnumerable<ProductId> Ids)
        : base(p => Ids.Contains(p.Id))
    {

    }

    public GetProductsSpecification WithNameCriteria(string? name)
    {
        AddCriteria(p => p.Name == name, !string.IsNullOrEmpty(name));
        return this;
    }

    public GetProductsSpecification WithProductTypeCriteria(ProductTypeId? productTypeId = null)
    {
        AddCriteria(p => p.ProductTypeId == productTypeId, productTypeId != null);
        return this;
    }

    public GetProductsSpecification WithBrandCriteria(BrandId? brandId = null)
    {
        AddCriteria(p => p.BrandId == brandId, brandId != null);
        return this;
    }
}
