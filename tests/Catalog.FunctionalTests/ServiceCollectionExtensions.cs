using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.FunctionalTests;

public static class ServiceCollectionExtensions
{
    public static void EnsureDbCreated<T>(this IServiceCollection services)
        where T : DbContext
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var dbContext = serviceProvider.GetRequiredService<T>();
        dbContext.Database.EnsureCreated();
    }

}
