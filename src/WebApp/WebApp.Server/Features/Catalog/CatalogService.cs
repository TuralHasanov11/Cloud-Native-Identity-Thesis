namespace WebApp.Server.Features.Catalog;

public class CatalogService(HttpClient httpClient) : ICatalogService
{
    public async Task<IEnumerable<Product>> GetProducts(IEnumerable<string> ids)
    {
        if (!ids.Any())
        {
            return Product.Empty();
        }

        var builder = new UriBuilder(httpClient.BaseAddress + "api/products/by")
        {
            Query = string.Join("&", ids.Select(id => $"ids={Uri.EscapeDataString(id)}"))
        };

        var response = await httpClient.GetFromJsonAsync<IEnumerable<Product>>(builder.Uri);

        return response ?? Product.Empty();
    }
}
