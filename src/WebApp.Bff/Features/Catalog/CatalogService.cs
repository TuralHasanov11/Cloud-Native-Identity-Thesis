
namespace WebApp.Bff.Features.Catalog;

public class CatalogService(HttpClient httpClient) : ICatalogService
{
    public Task<IEnumerable<Product>> GetProducts(IEnumerable<string> ids)
    {
        throw new NotImplementedException();
    }
}
