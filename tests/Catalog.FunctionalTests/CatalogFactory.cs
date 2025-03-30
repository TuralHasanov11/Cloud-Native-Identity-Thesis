using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;

namespace Catalog.FunctionalTests;

public class CatalogFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithDatabase("catalog")
        .Build();

    private DbConnection _dbConnection = default!;

#pragma warning disable IDE0052 // Remove unread private members
    private Respawner _respawner = default!;
#pragma warning restore IDE0052 // Remove unread private members

    public HttpClient HttpClient { get; private set; } = default!;

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());

        HttpClient = CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });

        //await InitializeRespawner();
    }

    private async Task InitializeRespawner()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(
            _dbConnection,
            new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = ["public"]
            });
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<CatalogDbContext>>();
            //services.RemoveAll<CatalogDbContext>();

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
