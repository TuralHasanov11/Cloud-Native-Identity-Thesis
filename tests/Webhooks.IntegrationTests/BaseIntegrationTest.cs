namespace Webhooks.IntegrationTests;

[Collection(nameof(IntegrationTestCollection))]
public class BaseIntegrationTest : IAsyncLifetime
{
    protected const string ApiBaseUrl = "https://localhost:5109";

    private readonly Func<Task> _resetDatabase;

    protected WebhooksDbContext DbContext { get; }

    private readonly IServiceScope _scope;

    protected BaseIntegrationTest(WebhooksFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _resetDatabase = factory.ResetDatabaseAsync;
        DbContext = _scope.ServiceProvider.GetRequiredService<WebhooksDbContext>();
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync() => await _resetDatabase();
}
