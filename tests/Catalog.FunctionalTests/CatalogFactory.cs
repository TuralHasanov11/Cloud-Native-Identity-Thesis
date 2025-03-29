using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Npgsql;
using Testcontainers.PostgreSql;

namespace Catalog.FunctionalTests;

public class CatalogFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithDatabase("ordering")
        .Build();

    private DbConnection _dbConnection = default!;

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        await _dbConnection.OpenAsync();

        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<CatalogDbContext>>();
            services.RemoveAll<CatalogDbContext>();

            services.AddDbContext<CatalogDbContext>(
                a => a.UseNpgsql(_dbContainer.GetConnectionString()),
                ServiceLifetime.Singleton);
        });
    }

    public async Task ResetDatabaseAsync()
    {
        //await _respawner.ResetAsync(_dbConnection);
    }


    public new async Task DisposeAsync()
    {
        await _dbConnection.DisposeAsync();
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }
}
