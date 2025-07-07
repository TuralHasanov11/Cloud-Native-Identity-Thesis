using System.IdentityModel.Tokens.Jwt;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.TokenCacheProviders.InMemory;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using ServiceDefaults.Identity;

namespace Webhooks.Client.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddAuthenticationServices();

        // Application services
        builder.Services.AddOptions<WebhookClientOptions>().BindConfiguration(nameof(WebhookClientOptions));
        builder.Services.AddSingleton<HooksRepository>();

        // HTTP client registrations
        builder.Services.AddHttpClient<WebhooksClient>(o => o.BaseAddress = new("https://webhooks-api:5109"));
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
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
            options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
            options.HandleSameSiteCookieCompatibility();
        });


        var defaultScopes = builder.Configuration[$"{IdentityProviderSettings.AzureAd}:Scopes"]
            ?.Split(" ", StringSplitOptions.RemoveEmptyEntries);

        builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(
                options =>
                {
                    builder.Configuration.Bind(IdentityProviderSettings.AzureAd, options);
                    options.TokenValidationParameters.RoleClaimType = "roles";
                },
                options =>
                {
                    builder.Configuration.Bind(IdentityProviderSettings.AzureAd, options);
                }
            )
            .EnableTokenAcquisitionToCallDownstreamApi(defaultScopes)
            .AddInMemoryTokenCaches();

        builder.Services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
        {
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
