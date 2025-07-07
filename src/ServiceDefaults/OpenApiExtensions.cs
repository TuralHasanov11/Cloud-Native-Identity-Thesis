using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

namespace ServiceDefaults;

public static partial class OpenApiExtensions
{
    public static IApplicationBuilder UseDefaultOpenApi(this WebApplication app)
    {
        var openApiInfoOptions = app.Configuration.GetSection("OpenApiInfo:v1").Get<OpenApiInfo>();

        if (openApiInfoOptions is null)
        {
            return app;
        }

        app.MapOpenApi().AllowAnonymous();

        if (app.Environment.IsDevelopment())
        {
            app.MapScalarApiReference(options =>
            {
                options.DefaultFonts = false;
            }).AllowAnonymous();

            app.MapGet("/", () => Results.Redirect("/scalar/v1"))
                .ExcludeFromDescription()
                .AllowAnonymous();
        }

        return app;
    }

    public static IHostApplicationBuilder AddDefaultOpenApi(
        this IHostApplicationBuilder builder,
        IApiVersioningBuilder? apiVersioning = default
        )
    {
        var openApiInfoOptions = builder.Configuration.GetSection("OpenApiInfo");
        var identitySettings = builder.Configuration.GetSection(IdentityProviderSettings.SectionName).Get<IdentityProviderSettings>();

        if (identitySettings is null)
        {
            return builder;
        }

        var enabledProvider = identitySettings.EnabledProviderName;

        if (enabledProvider is null)
        {
            return builder;
        }

        string[] scopes = [];

        if (openApiInfoOptions is null)
        {
            return builder;
        }

        if (apiVersioning is not null)
        {
            foreach (var description in (string[])["v1", "v2"])
            {
                var versionedOpenApiInfo = openApiInfoOptions.GetSection(description).Get<OpenApiInfo>();

                if (versionedOpenApiInfo is not null)
                {
                    builder.Services.AddOpenApi(description, options =>
                    {
                        options.ApplyApiVersionInfo(versionedOpenApiInfo)
                            .ApplySecuritySchemeDefinitions(scopes)
                            .ApplyOperationDeprecatedStatus();
                    });
                }
            }
        }

        return builder;
    }
}
