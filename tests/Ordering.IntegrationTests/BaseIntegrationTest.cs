namespace Ordering.IntegrationTests;

public class BaseIntegrationTest
{
    protected OrderingDbContext DbContext { get; }

    public HttpClient HttpClient { get; private set; } = default!;

    protected BaseIntegrationTest(OrderingFactory factory)
    {
        DbContext = factory.Services.GetRequiredService<OrderingDbContext>();
        HttpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });
    }
}
