using Serilog;
using ServiceDefaults.Middleware;
using WebApp.Bff.Extensions;
using WebApp.Bff.Features;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .CreateLogger();

try
{
    Log.Information("Starting WebApp.Bff");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseDefaultServiceProvider(config => config.ValidateOnBuild = true);
    builder.WebHost.UseKestrel(options => options.AddServerHeader = false);

    builder.AddServiceDefaults();

    builder.AddApplicationServices();

    var app = builder.Build();

    app.UseDefaultLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();

    app.UseMiddleware<ContentTypeOptionsMiddleware>();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseStatusCodePages();

    app.MapDefaultEndpoints();

    app.UseDefaultOpenApi();

    app.MapReverseProxy();

    //app.MapForwarder("/{**catch-all}", app.Configuration["ClientUrl"]!);

    app.MapBffApi();

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
