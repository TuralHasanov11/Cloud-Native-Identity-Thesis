namespace Ordering.IntegrationTests;

public class BaseIntegrationTest : IDisposable
{
    protected IServiceScope Scope { get; private set; }
    private bool _disposed = false;

    public HttpClient HttpClient { get; private set; } = default!;

    protected BaseIntegrationTest(OrderingFactory factory)
    {
        Scope = factory.Services.CreateScope();
        HttpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                Scope.Dispose();
                HttpClient.Dispose();
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
