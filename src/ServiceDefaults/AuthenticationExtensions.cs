using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ServiceDefaults;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddDefaultAuthentication(this IHostApplicationBuilder builder)
    {
        var identitySettings = builder.Configuration
            .GetSection(IdentitySettings.SectionName)
            .Get<IdentitySettings>();

        if (identitySettings is null)
        {
            return builder.Services;
        }

        var enabledProvider = identitySettings.EnabledProvider;

        if (enabledProvider is null)
        {
            return builder.Services;
        }

        builder.Services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.Authority = enabledProvider.Authority;
                options.Audience = identitySettings?.Audience;
                options.RequireHttpsMetadata = false;
                options.MapInboundClaims = false;
                options.TokenValidationParameters.ValidIssuer = enabledProvider.Authority;
                //options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Sub;
                //options.TokenValidationParameters.RoleClaimType = JwtRegisteredClaimNames.Sub;
                options.TokenValidationParameters.ValidateAudience = true;
                options.TokenValidationParameters.ValidateIssuer = true;
                options.TokenValidationParameters.ValidateLifetime = true;
                options.TokenValidationParameters.ValidateIssuerSigningKey = true;
                options.TokenValidationParameters.ValidateTokenReplay = true;

                if (builder.Environment.IsDevelopment())
                {
                    options.IncludeErrorDetails = true;
                }
            });

        builder.Services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            options.FallbackPolicy = options.DefaultPolicy;
        });

        return builder.Services;
    }
}

public class IdentitySettings
{
    public const string SectionName = "Identity";

    [Required]
    public string Audience { get; set; }

    [Required]
    public IReadOnlyDictionary<string, IdentityProviderSettings> Providers { get; set; }
        = new Dictionary<string, IdentityProviderSettings>();

    public IdentityProviderSettings? EnabledProvider => Providers.FirstOrDefault(p => p.Value.Enabled).Value;

    public class IdentityProviderSettings
    {
        public bool Enabled { get; set; }

        public string Authority { get; set; }

        public IReadOnlyDictionary<string, string?> Scopes { get; set; } = new Dictionary<string, string?>();
    }
}
