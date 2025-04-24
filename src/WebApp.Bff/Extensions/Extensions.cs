using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.TokenCacheProviders.InMemory;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
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

        var azureScopes = builder.Configuration.GetSection("AzureScopes").Get<Dictionary<string, string>>();

        builder.Services.AddReverseProxy()
           .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
           .AddTransforms(context =>
           {
               context.AddRequestTransform(async request =>
               {
                   if (azureScopes is not null)
                   {
                       ITokenAcquisition tokenAcquisition = context.Services.GetRequiredService<ITokenAcquisition>();
                       var accessToken = await tokenAcquisition.GetAccessTokenForUserAsync(azureScopes.Values);

                       Debug.WriteLine($"access token-{accessToken}");

                       if (accessToken is not null)
                       {
                           request.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                           request.ProxyRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                       }
                   }
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
        builder.Services.AddGrpcClient<Basket.Api.Grpc.Basket.BasketClient>(
            o => o.Address = new("https://basket.api:5101"))
            // TODO: Remove on production
            .ConfigureChannel(options =>
            {
                options.HttpHandler = new SocketsHttpHandler
                {
                    SslOptions = new System.Net.Security.SslClientAuthenticationOptions
                    {
                        RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
                    }
                };
            });

        builder.Services.AddHttpClient<ICatalogService, CatalogService>(o => o.BaseAddress = new("https://catalog.api:5103"))
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            })
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

        var azureAdB2CInstance = builder.Configuration.GetSection($"{IdentityConstants.AzureAdB2CScheme}:Instance").Get<string>();
        ArgumentNullException.ThrowIfNull(azureAdB2CInstance, nameof(azureAdB2CInstance));


        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: Policies.DefaultCorsPolicy, policy =>
            {
                policy.WithOrigins(
                        azureAdB2CInstance,
                        clientUrl,
                        "http://webapp:5173",
                        "http://localhost:5173",
                        "http://webapp:5174",
                        "http://localhost:5174")
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }

    private static void AddAuthenticationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddInMemoryTokenCaches();

        builder.Services.AddOptions<OpenIdConnectOptions>()
            .BindConfiguration(IdentityConstants.AzureAdB2CScheme)
            .ValidateOnStart()
            .ValidateDataAnnotations();

        var azureScopes = builder.Configuration.GetSection("AzureScopes").Get<Dictionary<string, string>>();

        builder.Services.Configure<CookiePolicyOptions>(options =>
        {
            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
            // Handling SameSite cookie according to https://learn.microsoft.com/aspnet/core/security/samesite?view=aspnetcore-3.1
            options.HandleSameSiteCookieCompatibility();
        });

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Add(JwtRegisteredClaimNames.Sub, ClaimTypes.NameIdentifier);
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Add("given_name", ClaimTypes.GivenName);

        builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, IdentityConstants.AzureAdB2CScheme)
            .EnableTokenAcquisitionToCallDownstreamApi(azureScopes?.Values)
            .AddInMemoryTokenCaches();

        builder.Services.AddControllersWithViews()
            .AddMicrosoftIdentityUI();

        builder.Services.AddAuthorization(policyBuilder =>
        {
            policyBuilder.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            policyBuilder.FallbackPolicy = policyBuilder.DefaultPolicy;
        });

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddTransient<IIdentityService, IdentityService>();
    }

    private static void AddAIServices(this IHostApplicationBuilder _)
    {

    }
}
