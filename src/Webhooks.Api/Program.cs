using ServiceDefaults.Middleware;
using Webhooks.Api.Extensions;
using Webhooks.Api.Features;

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

    app.UseExceptionHandler();

    if (!app.Environment.IsDevelopment())
    {
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseMiddleware<ContentTypeOptionsMiddleware>();

    app.UseCors("CorsPolicy");

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseStatusCodePages();

    app.MapDefaultEndpoints();

    app.MapWebhooksApi();

    app.UseDefaultOpenApi();

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

public partial class Program
{
    protected Program()
    {
    }
}
