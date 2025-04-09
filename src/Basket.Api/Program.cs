using Basket.Api.Extensions;
using Basket.Api.Features;
using Basket.Api.Features.Basket;
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

    builder.Services.AddGrpc();

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

    //app.UseRateLimiter();
    //app.UseRequestLocalization();
    app.UseCors("CorsPolicy");

    //app.UseOutputCache();

    //app.UseRequestDecompression();

    app.UseAuthentication();
    app.UseAuthorization();
    //app.UseResponseCompression();

    app.UseStatusCodePages();

    app.MapDefaultEndpoints();

    app.MapGrpcService<BasketService>();
    app.MapBasketEndpoints();

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

public partial class Program;
