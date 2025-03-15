using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace ServiceDefaults;

public static partial class Extensions
{
    public static IApplicationBuilder UseDefaultOpenApi(this WebApplication app)
    {
        var openApiInfoOptions = app.Configuration.GetSection("OpenApiInfo:v1").Get<OpenApiInfo>();

        if (openApiInfoOptions is null)
        {
            return app;
        }

        app.MapOpenApi();

        if (app.Environment.IsDevelopment())
        {
            //app.MapScalarApiReference(options =>
            //{
            //    // Disable default fonts to avoid download unnecessary fonts
            //    options.DefaultFonts = false;
            //});
            //app.MapGet("/", () => Results.Redirect("/scalar/v1")).ExcludeFromDescription();
        }

        return app;
    }

    public static IHostApplicationBuilder AddDefaultOpenApi(
        this IHostApplicationBuilder builder,
        IApiVersioningBuilder? apiVersioning = default
        )
    {
        var openApiInfoOptions = builder.Configuration.GetSection("OpenApiInfo");
        var identitySettings = builder.Configuration.GetSection("Identity").Get<IdentitySettings>();

        var scopes = identitySettings is not null
            ? identitySettings.Scopes
            : new Dictionary<string, string?>();


        if (openApiInfoOptions is null)
        {
            return builder;
        }

        if (apiVersioning is not null)
        {
            foreach (var description in (string[])["v1", "v2"])
            {
                var versionedOpenApiInfo = openApiInfoOptions.GetRequiredSection(description).Get<OpenApiInfo>();

                if (versionedOpenApiInfo is not null)
                {
                    builder.Services.AddOpenApi(description, options =>
                    {
                        options.ApplyApiVersionInfo(versionedOpenApiInfo);
                        //options.ApplyAuthorizationChecks([.. scopes.Keys]);
                        //options.ApplySecuritySchemeDefinitions();
                        options.ApplyOperationDeprecatedStatus();
                        //options.ApplyApiVersionDescription();
                        //options.ApplySchemaNullableFalse();
                    });
                }
            }
        }

        return builder;
    }
}
