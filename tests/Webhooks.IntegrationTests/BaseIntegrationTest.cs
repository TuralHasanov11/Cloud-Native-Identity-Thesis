using Microsoft.AspNetCore.Mvc.Testing;

namespace Webhooks.IntegrationTests;

public class BaseIntegrationTest
{
    protected WebhooksDbContext DbContext { get; }

    public HttpClient HttpClient { get; private set; } = default!;

    protected BaseIntegrationTest(WebhooksFactory factory)
    {
        DbContext = factory.Services.GetRequiredService<WebhooksDbContext>();
        HttpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });
    }
}

