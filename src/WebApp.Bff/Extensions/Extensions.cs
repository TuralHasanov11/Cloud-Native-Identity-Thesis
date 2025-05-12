using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.TokenCacheProviders.InMemory;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Polly;
using ServiceDefaults.Identity;
using WebApp.Bff.Features.Basket;
using WebApp.Bff.Features.Catalog;
using WebApp.Bff.Features.Identity;
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
                   //if (azureScopes is not null)
                   //{
                   //    ITokenAcquisition tokenAcquisition = context.Services.GetRequiredService<ITokenAcquisition>();
                   //    var accessToken = await tokenAcquisition.GetAccessTokenForUserAsync(azureScopes.Values);

                   //    Debug.WriteLine($"access token-{accessToken}");

                   //    if (accessToken is not null)
                   //    {
                   //        request.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                   //        request.ProxyRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                   //    }
                   //}
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

        var azureAdB2CInstance = builder.Configuration
            .GetSection($"{IdentityProviderSettings.AzureAdB2C}:Instance")
            .Get<string>();
        ArgumentNullException.ThrowIfNull(azureAdB2CInstance, nameof(azureAdB2CInstance));

        var awsCognitoInstance = builder.Configuration
            .GetSection($"{IdentityProviderSettings.AWSCognito}:Authority")
            .Get<string>();
        ArgumentNullException.ThrowIfNull(awsCognitoInstance, nameof(awsCognitoInstance));

        var googleCloudIdentityInstance = builder.Configuration
            .GetSection($"{IdentityProviderSettings.GoogleCloudIdentity}:Authority")
            .Get<string>();
        ArgumentNullException.ThrowIfNull(googleCloudIdentityInstance, nameof(googleCloudIdentityInstance));

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: Policies.DefaultCorsPolicy, policy =>
            {
                policy.WithOrigins(
                        clientUrl,
                        azureAdB2CInstance,
                        awsCognitoInstance,
                        googleCloudIdentityInstance,
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

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Add(JwtRegisteredClaimNames.Sub, ClaimTypes.NameIdentifier);
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Add("given_name", ClaimTypes.GivenName);

        // Azure
        //AddAzureAdB2C(builder);

        // AWS
        //AddAWSCognito(builder);

        // GCP
        AddGoogleCloudIdentity(builder);

        builder.Services.AddAuthorization();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddTransient<IIdentityService, IdentityService>();
    }

    private static void AddGoogleCloudIdentity(IHostApplicationBuilder builder)
    {
        var googleCloudIdentitySettings = builder.Configuration
            .GetSection(IdentityProviderSettings.GoogleCloudIdentity);

        builder.Services
            .AddAuthentication(o =>
            {
                o.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
                o.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
                o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddGoogleOpenIdConnect(options =>
            {
                options.ClientId = googleCloudIdentitySettings["ClientId"];
                options.ClientSecret = googleCloudIdentitySettings["ClientSecret"];
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
            });
    }

    private static void AddAzureAdB2C(IHostApplicationBuilder builder)
    {
        builder.Services.Configure<CookiePolicyOptions>(options =>
        {
            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
            // Handling SameSite cookie according to https://learn.microsoft.com/aspnet/core/security/samesite?view=aspnetcore-3.1
            options.HandleSameSiteCookieCompatibility();
        });

        builder.Services.AddOptions<OpenIdConnectOptions>()
            .BindConfiguration(IdentityProviderSettings.AzureAdB2C)
            .ValidateOnStart()
            .ValidateDataAnnotations();

        var defaultScopes = builder.Configuration[$"{IdentityProviderSettings.AzureAdB2C}:Scopes"]
            ?.Split(" ", StringSplitOptions.RemoveEmptyEntries);

        builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, IdentityProviderSettings.AzureAdB2C)
            .EnableTokenAcquisitionToCallDownstreamApi(defaultScopes)
            .AddInMemoryTokenCaches();

        builder.Services.AddControllersWithViews()
            .AddMicrosoftIdentityUI();
    }

    private static void AddAWSCognito(IHostApplicationBuilder builder)
    {
        builder.Services.AddOptions<OpenIdConnectOptions>()
            .BindConfiguration(IdentityProviderSettings.AWSCognito)
            .ValidateOnStart()
            .ValidateDataAnnotations();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie()
        .AddOpenIdConnect(options =>
        {
            var oidcConfig = builder.Configuration.GetSection("AWSCognito");

            options.Authority = oidcConfig["Authority"];
            options.ClientId = oidcConfig["ClientId"];
            options.ClientSecret = oidcConfig["ClientSecret"];
            options.MetadataAddress = oidcConfig["MetadataAddress"];

            var scopes = oidcConfig["Scopes"]?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            options.Scope.Clear();
            if (scopes is not null)
            {
                foreach (var scope in scopes)
                {
                    options.Scope.Add(scope);
                }
            }

            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.ResponseType = OpenIdConnectResponseType.Code;
            options.UsePkce = true;

            options.SaveTokens = true;
            options.GetClaimsFromUserInfoEndpoint = true;

            //options.MapInboundClaims = false;
            //options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
            //options.TokenValidationParameters.RoleClaimType = "roles";
        });

        builder.Services.ConfigureCookieOidcRefresh(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
    }

    private static void AddAIServices(this IHostApplicationBuilder _)
    {

    }

    public static IServiceCollection ConfigureCookieOidcRefresh(this IServiceCollection services, string cookieScheme, string oidcScheme)
    {
        services.AddSingleton<CookieOidcRefresher>();
        services.AddOptions<CookieAuthenticationOptions>(cookieScheme).Configure<CookieOidcRefresher>((cookieOptions, refresher) =>
        {
            cookieOptions.Events.OnValidatePrincipal = context => refresher.ValidateOrRefreshCookieAsync(context, oidcScheme);
        });
        services.AddOptions<OpenIdConnectOptions>(oidcScheme).Configure(oidcOptions =>
        {
            // Request a refresh_token.
            oidcOptions.Scope.Add(OpenIdConnectScope.OfflineAccess);
            // Store the refresh_token.
            oidcOptions.SaveTokens = true;
        });
        return services;
    }
}
