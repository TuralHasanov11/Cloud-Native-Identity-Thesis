using Catalog.Api.Extensions;
using Catalog.Api.Features;
using Serilog;
using ServiceDefaults.Middleware;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .CreateLogger();

try
{
    Log.Information("Starting web host");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseDefaultServiceProvider(config => config.ValidateOnBuild = true);
    builder.WebHost.UseKestrel(options => options.AddServerHeader = false);

    builder.AddServiceDefaults();
    builder.AddApplicationServices();

    var withApiVersioning = builder.Services.AddApiVersioning();
    builder.AddDefaultOpenApi(withApiVersioning);

    var app = builder.Build();

    app.UseDefaultLogging();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseMiddleware<ContentTypeOptionsMiddleware>();

    //app.UseRateLimiter();
    //app.UseRequestLocalization();
    //app.UseCors(Policies.DefaultCorsPolicy);

    //app.UseOutputCache();

    //app.UseRequestDecompression();

    app.UseAuthentication();
    app.UseAuthorization();
    //app.UseResponseCompression();

    app.MapDefaultEndpoints();

    app.UseStatusCodePages();

    app.MapCatalogApi();

    app.UseDefaultOpenApi();

    app.UseBackgroundJobs();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Error(ex, "Application terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}

public partial class Program;
