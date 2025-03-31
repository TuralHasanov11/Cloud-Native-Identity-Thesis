namespace Catalog.IntegrationTests;

public class BaseIntegrationTest : IClassFixture<CatalogFactory>, IAsyncLifetime
{
    protected const string ApiBaseUrl = "https://localhost:5103";

    private readonly Func<Task> _resetDatabase;

#pragma warning disable CA1051 // Do not declare visible instance fields
    protected readonly CatalogDbContext DbContext;
#pragma warning restore CA1051 // Do not declare visible instance fields

    private readonly IServiceScope _scope;

    protected BaseIntegrationTest(CatalogFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _resetDatabase = factory.ResetDatabaseAsync;
        DbContext = _scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

        var pendingMigrations = DbContext.Database.GetPendingMigrations();

        if (pendingMigrations.Any())
        {
            Console.WriteLine("Applying pending migrations...");
            DbContext.Database.Migrate();
        }
        else
        {
            Console.WriteLine("No pending migrations.");
        }
    }

    private static IEnumerable<Brand> GetBrands()
    {
        yield return Brand.Create("Brand 1");
        yield return Brand.Create("Brand 2");
        yield return Brand.Create("Brand 3");
        yield return Brand.Create("Brand 4");
    }

    private async Task SeedDatabase()
    {
        if (!await DbContext.Products.AnyAsync())
        {
            //await DbContext.Brands.AddRangeAsync(GetBrands());
            //await DbContext.SaveChangesAsync();
        }
    }

    public async Task InitializeAsync()
    {
        await SeedDatabase();
    }

    public async Task DisposeAsync() => await _resetDatabase();
}
