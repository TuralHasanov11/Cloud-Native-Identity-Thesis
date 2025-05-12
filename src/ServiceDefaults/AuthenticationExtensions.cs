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

        if (identitySettings.EnabledProviderName == IdentityProviderSettings.AzureAdB2C)
        {
            AddAzureADB2C(builder);
        }
        else if (identitySettings.EnabledProviderName == IdentityProviderSettings.AWSCognito)
        {
            AddAWSCognito(builder);
        }

        return builder.Services;
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
                var jwtOptions = builder.Configuration.GetSection(IdentityProviderSettings.AWSCognito).Get<JwtBearerOptions>()!;

                options.MetadataAddress = jwtOptions.MetadataAddress;
                options.Authority = jwtOptions.Authority;
                options.Audience = jwtOptions.Audience;
                options.IncludeErrorDetails = jwtOptions.IncludeErrorDetails;
                options.RequireHttpsMetadata = jwtOptions.RequireHttpsMetadata;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = jwtOptions.TokenValidationParameters.ValidateIssuer,
                    ValidateAudience = jwtOptions.TokenValidationParameters.ValidateAudience,
                    ValidateIssuerSigningKey = jwtOptions.TokenValidationParameters.ValidateIssuerSigningKey,
                    ValidateLifetime = jwtOptions.TokenValidationParameters.ValidateLifetime,
                    ValidIssuer = jwtOptions.Authority,
                };

                options.MapInboundClaims = jwtOptions.MapInboundClaims;
            });

        //builder.Services.AddSingleton<IAuthorizationPolicyProvider, BaseAuthorizationPolicyProvider>();
        builder.Services.AddSingleton<IAuthorizationHandler, GroupHandler>();
        builder.Services.AddAuthorization();
    }

    private static void AddAzureADB2C(IHostApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(
                options =>
                {
                    builder.Configuration.Bind("AzureAdB2C", options);
                    options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Sub;
                },
                options =>
                {
                    builder.Configuration.Bind("AzureAdB2C", options);
                    options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Sub;
                });

        builder.Services.AddSingleton<IAuthorizationPolicyProvider, BaseAuthorizationPolicyProvider>();
        builder.Services.AddSingleton<IAuthorizationHandler, GroupHandler>();
        builder.Services.AddAuthorization();
    }
}

public class IdentityProviderSettings : Dictionary<string, bool?>
{
    public const string SectionName = "IdentityProviders";

    public const string AzureAdB2C = "AzureAdB2C";
    public const string AWSCognito = "AWSCognito";
    public const string GoogleCloudIdentity = "GoogleCloudIdentity";

    public string EnabledProviderName => this.FirstOrDefault(x => x.Value == true).Key;
}
