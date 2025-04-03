using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .CreateLogger();

try
{
    Log.Information("Starting web host");

    var builder = WebApplication.CreateBuilder(args);

    builder.AddServiceDefaults();

    builder.AddApplicationServices();

    builder.Services.AddRazorPages();

    var app = builder.Build();

    app.UseExceptionHandler("/Error");

    if (!app.Environment.IsDevelopment())
    {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseAntiforgery();

    app.UseRouting();

    app.UseAuthorization();

    app.MapStaticAssets();
    app.MapRazorPages()
       .WithStaticAssets();

    app.UseStatusCodePages();

    app.MapDefaultEndpoints();

    app.MapAuthenticationEndpoints();

    app.MapWebhookEndpoints();

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
