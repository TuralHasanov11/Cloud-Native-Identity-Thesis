using Microsoft.AspNetCore.Mvc.Testing;

namespace Catalog.IntegrationTests;

public class BaseIntegrationTest
{
    protected const string ApiBaseUrl = "https://localhost:5103";

    protected CatalogDbContext DbContext { get; }

    public HttpClient HttpClient { get; private set; } = default!;

    private readonly IServiceScope _scope;

    protected BaseIntegrationTest(CatalogFactory factory)
    {
        _scope = factory.Services.CreateScope();
        DbContext = factory.Services.GetRequiredService<CatalogDbContext>();
        HttpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });
    }
}
