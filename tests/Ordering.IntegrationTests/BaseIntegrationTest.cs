namespace Ordering.IntegrationTests;

[Collection(nameof(IntegrationTestCollection))]
public class BaseIntegrationTest : IAsyncLifetime
{
    private readonly Func<Task> _resetDatabase;

    protected OrderingDbContext DbContext { get; }

    private readonly IServiceScope _scope;

    protected BaseIntegrationTest(OrderingFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _resetDatabase = factory.ResetDatabaseAsync;
        DbContext = _scope.ServiceProvider.GetRequiredService<OrderingDbContext>();
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync() => await _resetDatabase();
}
