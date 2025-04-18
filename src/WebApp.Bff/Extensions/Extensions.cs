using ServiceDefaults.Identity;
using Yarp.ReverseProxy.Transforms;

namespace WebApp.Bff.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddAuthenticationServices();

        builder.Services.AddReverseProxy()
           .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
           .AddTransforms(context =>
           {
               context.AddRequestTransform(async request =>
               {
                   Console.WriteLine(request);
                   //var token = await request.HttpContext.GetUserAccessTokenAsync();
                   //request.ProxyRequest.Headers.Authorization =
                   //        new AuthenticationHeaderValue("Bearer", token.AccessToken);
               });
           })
        .AddCorrelationId()
        .ConfigureHttpClient((context, handler) =>
        {
            if (builder.Configuration.GetValue<bool>("ReverseProxy:HttpClient:DangerousAcceptAnyServerCertificate"))
            {
                handler.SslOptions.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            }
        });

        AddCors(builder);
    }

    private static void AddCors(IHostApplicationBuilder builder)
    {
        var clientUrl = builder.Configuration.GetSection("ClientUrl").Get<string>();
        ArgumentNullException.ThrowIfNull(clientUrl);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: Policies.DefaultCorsPolicy, policy =>
            {
                policy.WithOrigins(
                        clientUrl,
                        "https://webapp:5173",
                        "https://localhost:5173",
                        "http://webapp:5173",
                        "http://localhost:5173")
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }

    public static void AddAuthenticationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddTransient<IIdentityService, IdentityService>();
    }

    private static void AddAIServices(this IHostApplicationBuilder _)
    {

    }
}
