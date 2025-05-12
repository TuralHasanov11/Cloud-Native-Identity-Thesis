using Microsoft.AspNetCore.Mvc.Testing;

namespace Catalog.IntegrationTests;

public class BaseIntegrationTest
{
    protected const string ApiBaseUrl = "https://localhost:5103";

    protected CatalogDbContext DbContext { get; }

    public HttpClient HttpClient { get; private set; } = default!;

    protected BaseIntegrationTest(CatalogFactory factory)
    {
        DbContext = factory.Services.GetRequiredService<CatalogDbContext>();
        HttpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });
    }
}
