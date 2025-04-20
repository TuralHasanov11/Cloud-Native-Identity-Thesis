namespace WebApp.Bff.Features.Catalog;

public interface ICatalogService
{
    Task<IEnumerable<Product>> GetProducts(IEnumerable<string> ids);
}
