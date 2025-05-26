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

    public ValueTask InitializeAsync()
    {
        return ValueTask.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        await _resetDatabase();
        _scope.Dispose();
        GC.SuppressFinalize(this);
    }
}
