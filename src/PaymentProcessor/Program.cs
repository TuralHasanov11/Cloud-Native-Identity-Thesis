using PaymentProcessor.Extensions;
using Serilog;
using ServiceDefaults;

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

    builder.Services.AddOptions<PaymentOptions>()
        .BindConfiguration(nameof(PaymentOptions))
        .ValidateDataAnnotations()
        .ValidateOnStart();

    var app = builder.Build();

    app.UseHttpsRedirection();

    app.MapDefaultEndpoints();

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
