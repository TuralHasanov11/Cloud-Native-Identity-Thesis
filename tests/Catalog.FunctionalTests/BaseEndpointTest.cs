namespace Catalog.FunctionalTests;

[Collection(nameof(EndpointTestCollection))]
public class BaseEndpointTest : IAsyncLifetime
{
    protected IServiceScope Scope { get; }

    protected const string ApiBaseUrl = "https://localhost:5103";

    private readonly Func<Task> _resetDatabase;

    protected BaseEndpointTest(CatalogFactory factory)
    {
        Scope = factory.Services.CreateScope();
        DbContext = Scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
        _resetDatabase = factory.ResetDatabaseAsync;
    }

    protected CatalogDbContext DbContext { get; }

    public async Task SeedDatabase()
    {
        if (!await DbContext.Products.AnyAsync())
        {
            var subjects = GetBrands();
            DbContext.Brands.AddRange(subjects);

            await DbContext.SaveChangesAsync();
        }

        await DbContext.SaveChangesAsync();
    }

    private static IEnumerable<Brand> GetBrands()
    {
        yield return Brand.Create("Brand 1");
        yield return Brand.Create("Brand 2");
        yield return Brand.Create("Brand 3");
        yield return Brand.Create("Brand 4");
        yield return Brand.Create("Brand 5");
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabase();
}
