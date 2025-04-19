using Microsoft.Extensions.Http.Resilience;
using Polly;
using ServiceDefaults.Identity;
using WebApp.Bff.Features.Basket;
using WebApp.Bff.Features.Catalog;
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

        //builder.Services.AddResiliencePipeline<string, BasketService>("basket-service-fallback");
        builder.Services.AddResiliencePipeline<string, IEnumerable<Product>>(
            "catalog-service-fallback",
            pipelineBuilder =>
            {
                pipelineBuilder.AddFallback(new Polly.Fallback.FallbackStrategyOptions<IEnumerable<Product>>
                {
                    FallbackAction = _ =>
                    {
                        return Outcome.FromResultAsValueTask(Product.Empty());
                    }
                });
            });


        builder.Services.AddSingleton<BasketService>();
        builder.AddAIServices();

        // HTTP and GRPC client registrations
        builder.Services.AddGrpcClient<Basket.Api.Grpc.Basket.BasketClient>(o => o.Address = new("https://basket.api:5001"))
            .AddAuthToken();

        builder.Services.AddHttpClient<CatalogService>(o => o.BaseAddress = new("https://catalog.api:5003"))
            .AddAuthToken()
            .AddResilienceHandler("catalog-service", b =>
            {
                b.AddTimeout(TimeSpan.FromSeconds(5));
                b.AddRetry(new HttpRetryStrategyOptions
                {
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromMilliseconds(500),
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true,
                });
                b.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
                {
                    FailureRatio = 0.9,
                    BreakDuration = TimeSpan.FromSeconds(5),
                    SamplingDuration = TimeSpan.FromSeconds(5),
                    MinimumThroughput = 5,
                });
            });

        //builder.Services.AddHttpClient<OrderingService>(o => o.BaseAddress = new("http://ordering-api"))
        //    .AddAuthToken();
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
