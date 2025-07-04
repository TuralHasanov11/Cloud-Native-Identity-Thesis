using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentValidation;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.TokenCacheProviders.InMemory;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Polly;
using ServiceDefaults.Identity;
using WebApp.Server.Features.Basket;
using WebApp.Server.Features.Catalog;
using WebApp.Server.Features.Identity;

namespace WebApp.Server.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(AssemblyReference.Assembly);

        builder.AddAuthenticationServices();

        builder.Services.AddReverseProxy()
            .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
            .AddTransforms<JwtTransformProvider>()
            .AddTransforms<CorrelationIdTransformProvider>()
            .ConfigureHttpClient((context, handler) =>
            {
                if (builder.Configuration.GetValue<bool>("ReverseProxy:HttpClient:DangerousAcceptAnyServerCertificate"))
                {
                    handler.SslOptions.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                }
            });

        AddCors(builder);

        builder.Services.AddResiliencePipeline<string, IEnumerable<BasketItem>>(
            "catalog-service-fallback",
            pipelineBuilder =>
            {
                pipelineBuilder.AddFallback(new Polly.Fallback.FallbackStrategyOptions<IEnumerable<BasketItem>>
                {
                    FallbackAction = _ =>
                    {
                        return Outcome.FromResultAsValueTask(BasketItem.Empty());
                    }
                });
            });


        builder.Services.AddSingleton<BasketService>();

        // HTTP and GRPC client registrations
        builder.Services.AddGrpcClient<Basket.Api.Grpc.Basket.BasketClient>(
            o => o.Address = new("https://basket.api:5101"))
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

        var azureAdInstance = builder.Configuration
            .GetSection($"{IdentityProviderSettings.AzureAd}:Authority")
            .Get<string>();
        ArgumentNullException.ThrowIfNull(azureAdInstance, nameof(azureAdInstance));

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
                        azureAdInstance,
                        awsCognitoInstance,
                        googleCloudIdentityInstance,
                        "http://localhost:5173")
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }

    private static void AddAuthenticationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddInMemoryTokenCaches();

        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

        var identitySettings = builder.Configuration
            .GetSection(IdentityProviderSettings.SectionName)
            .Get<IdentityProviderSettings>();

        if (identitySettings is null)
        {
            return;
        }

        if (identitySettings.EnabledProviderName == IdentityProviderSettings.AzureAd)
        {
            AddMicrosoftEntraExternalId(builder);
        }
        else if (identitySettings.EnabledProviderName == IdentityProviderSettings.AWSCognito)
        {
            AddAWSCognito(builder);
        }
        else if (identitySettings.EnabledProviderName == IdentityProviderSettings.GoogleCloudIdentity)
        {
            AddGoogleCloudIdentity(builder);
        }

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

        builder.Services.AddControllersWithViews();
    }

    private static void AddMicrosoftEntraExternalId(IHostApplicationBuilder builder)
    {

        builder.Services.Configure<CookiePolicyOptions>(options =>
        {
            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
            options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
            // Handling SameSite cookie according to https://learn.microsoft.com/aspnet/core/security/samesite?view=aspnetcore-3.1
            options.HandleSameSiteCookieCompatibility();
        });


        var defaultScopes = builder.Configuration[$"{IdentityProviderSettings.AzureAd}:Scopes"]
            ?.Split(" ", StringSplitOptions.RemoveEmptyEntries);

        builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(builder.Configuration)
            .EnableTokenAcquisitionToCallDownstreamApi(defaultScopes)
            .AddInMemoryTokenCaches();

        builder.Services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
        {
            // The claim in the Jwt token where App roles are available.
            options.TokenValidationParameters.RoleClaimType = "roles";
        });

        builder.Services.AddControllersWithViews()
            .AddMicrosoftIdentityUI();
    }

    private static void AddAWSCognito(IHostApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie()
        .AddOpenIdConnect(options =>
        {
            var oidcConfig = builder.Configuration.GetSection(IdentityProviderSettings.AWSCognito);

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

            options.ResponseType = OpenIdConnectResponseType.Code;
            options.UsePkce = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = oidcConfig["Authority"],
            };
            options.SaveTokens = true;

            options.MapInboundClaims = false;
            options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
            options.TokenValidationParameters.RoleClaimType = "roles";
        });

        builder.Services.AddControllersWithViews();
    }
}
