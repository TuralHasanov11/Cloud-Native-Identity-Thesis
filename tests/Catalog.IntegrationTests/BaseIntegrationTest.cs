namespace Catalog.IntegrationTests;

[Collection(nameof(IntegrationTestCollection))]
public class BaseIntegrationTest : IAsyncLifetime
{
    protected const string ApiBaseUrl = "https://localhost:5103";

    private readonly Func<Task> _resetDatabase;

    protected CatalogDbContext DbContext { get; }

    private readonly IServiceScope _scope;

    protected BaseIntegrationTest(CatalogFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _resetDatabase = factory.ResetDatabaseAsync;
        DbContext = _scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync() => await _resetDatabase();
}
