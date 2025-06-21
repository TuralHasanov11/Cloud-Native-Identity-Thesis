namespace Catalog.IntegrationTests;

public class BaseIntegrationTest
{
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
