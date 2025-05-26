using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using ServiceDefaults.Identity;

namespace ServiceDefaults;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddDefaultAuthentication(this IHostApplicationBuilder builder)
    {
        var identitySettings = builder.Configuration
            .GetSection(IdentityProviderSettings.SectionName)
            .Get<IdentityProviderSettings>();

        if (identitySettings is null)
        {
            return builder.Services;
        }

        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

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
            AddGoogleIdentityPlatform(builder);
        }

        return builder.Services;
    }

    private static void AddGoogleIdentityPlatform(IHostApplicationBuilder _)
    {
        return;
    }

    private static void AddAWSCognito(IHostApplicationBuilder builder)
    {
        builder.Services.AddOptions<JwtBearerOptions>()
            .BindConfiguration(IdentityProviderSettings.AWSCognito)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                builder.Configuration.Bind(IdentityProviderSettings.AWSCognito, options);
            });

        //builder.Services.AddSingleton<IAuthorizationPolicyProvider, BaseAuthorizationPolicyProvider>();
        builder.Services.AddSingleton<IAuthorizationHandler, GroupHandler>();

        builder.Services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            options.FallbackPolicy = options.DefaultPolicy;
        });
    }

    private static void AddMicrosoftEntraExternalId(IHostApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(
                options =>
                {
                    builder.Configuration.Bind(IdentityProviderSettings.AzureAd, options);
                    //options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Sub;
                    options.TokenValidationParameters.RoleClaimType = "roles";
                },
                options =>
                {
                    builder.Configuration.Bind(IdentityProviderSettings.AzureAd, options);
                    //options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Sub;
                    options.TokenValidationParameters.RoleClaimType = "roles";
                });

        builder.Services.AddSingleton<IAuthorizationPolicyProvider, BaseAuthorizationPolicyProvider>();
        builder.Services.AddSingleton<IAuthorizationHandler, GroupHandler>();

        builder.Services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            options.FallbackPolicy = options.DefaultPolicy;

            options.AddPolicy("RoleOrderAdmins", policy =>
            {
                policy.RequireScopeOrAppPermission(allowedScopeValues: ["Ordering.ReadWrite"], allowedAppPermissionValues: [])
                    .RequireRole("Order.Admins");
            });

            options.AddPolicy("RoleCatalogAdmins", policy =>
            {
                policy.RequireScopeOrAppPermission(allowedScopeValues: ["Catalog.ReadWrite"], allowedAppPermissionValues: [])
                    .RequireRole("Catalog.Admins");
            });
        });
    }
}

public class IdentityProviderSettings : Dictionary<string, bool?>
{
    public const string SectionName = "IdentityProviders";

    public const string AzureAd = "AzureAd";
    public const string AWSCognito = "AWSCognito";
    public const string GoogleCloudIdentity = "GoogleCloudIdentity";

    public string EnabledProviderName => this.FirstOrDefault(x => x.Value == true).Key;
}
