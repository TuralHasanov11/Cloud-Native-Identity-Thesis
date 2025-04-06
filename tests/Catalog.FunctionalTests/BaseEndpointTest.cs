using Microsoft.AspNetCore.Mvc.Testing;

namespace Catalog.FunctionalTests;

[Collection(nameof(EndpointTestCollection))]
public class BaseEndpointTest : IAsyncLifetime
{
    private readonly Func<Task> _resetDatabase;

    protected CatalogDbContext DbContext { get; }

    private readonly IServiceScope _scope;

    protected HttpClient HttpClient { get; init; }

    protected BaseEndpointTest(CatalogFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _resetDatabase = factory.ResetDatabaseAsync;
        DbContext = _scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
        HttpClient = factory.CreateClient();
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync() => await _resetDatabase();
}
