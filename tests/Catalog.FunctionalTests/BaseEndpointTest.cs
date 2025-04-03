using Microsoft.AspNetCore.Mvc.Testing;

namespace Catalog.FunctionalTests;

[Collection(nameof(EndpointTestCollection))]
public class BaseEndpointTest : IAsyncLifetime
{
    protected const string ApiBaseUrl = "https://localhost:5103";

    private readonly Func<Task> _resetDatabase;

    protected WebApplicationFactory<Program> Factory { get; }

    protected BaseEndpointTest(CatalogFactory factory)
    {
        Factory = factory;
        _resetDatabase = factory.ResetDatabaseAsync;

        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

        var pendingMigrations = dbContext.Database.GetPendingMigrations();

        if (pendingMigrations.Any())
        {
            Console.WriteLine("Applying pending migrations...");
            dbContext.Database.Migrate();
        }
        else
        {
            Console.WriteLine("No pending migrations.");
        }
    }

    public async Task SeedDatabase()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

        if (!dbContext.Products.Any())
        {
            var brands = GetBrands();
            dbContext.Brands.AddRange(brands);
            dbContext.SaveChanges();
        }
    }

    private static IEnumerable<Brand> GetBrands()
    {
        yield return Brand.Create("Brand 1");
        yield return Brand.Create("Brand 2");
        yield return Brand.Create("Brand 3");
        yield return Brand.Create("Brand 4");
    }

    public Task InitializeAsync()
    {
        SeedDatabase();

        return Task.CompletedTask;
    }

    public async Task DisposeAsync() => await _resetDatabase();
}
