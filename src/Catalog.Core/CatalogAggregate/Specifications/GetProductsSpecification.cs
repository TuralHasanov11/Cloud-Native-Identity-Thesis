namespace Catalog.Core.CatalogAggregate.Specifications;

public class GetProductsSpecification : Specification<Product>
{
    public GetProductsSpecification(string? name, Guid? productTypeId, Guid? brandId)
    {
        AddCriteria(p => p.Name == name, name != null);

        if (productTypeId != null)
        {
            //AddCriteria(p => p.ProductTypeId == new ProductTypeId(productTypeId));
        }

        if (brandId != null)
        {
            //AddCriteria(p => p.BrandId == new BrandId(brandId));
        }
    }
}
