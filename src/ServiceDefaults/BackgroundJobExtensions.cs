using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel;

namespace ServiceDefaults;

public static class BackgroundJobExtensions
{
    public static IApplicationBuilder UseBackgroundJobs(this WebApplication app)
    {
        app.Services.GetRequiredService<IRecurringJobManager>()
           .AddOrUpdate<IOutboxProcessor>(
                "outbox-processor",
                job => job.ExecuteAsync(CancellationToken.None),
                "0/15 * * * * *");

        return app;
    }
}
