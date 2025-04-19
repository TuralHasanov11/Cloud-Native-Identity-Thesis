
namespace WebApp.Bff.Features.Catalog;

public class CatalogService : ICatalogService
{
    public Task<IEnumerable<Product>> GetProducts(IEnumerable<string> ids)
    {
        throw new NotImplementedException();
    }
}
