using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace ServiceDefaults;

internal static class OpenApiOptionsExtensions
{
    public static OpenApiOptions ApplyApiVersionInfo(this OpenApiOptions options, OpenApiInfo openApiInfo)
    {
        options.AddDocumentTransformer((document, context, cancellationToken) =>
        {
            var versionedDescriptionProvider = context.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
            var apiDescription = versionedDescriptionProvider?.ApiVersionDescriptions
                .SingleOrDefault(description => description.GroupName == context.DocumentName);

            if (apiDescription is null)
            {
                return Task.CompletedTask;
            }

            document.Info = new()
            {
                Title = openApiInfo.Title,
                Version = openApiInfo.Version,
                Description = openApiInfo.Description,
                Contact = new()
                {
                    Name = openApiInfo.Contact.Name,
                    Email = openApiInfo.Contact.Email,
                    Url = openApiInfo.Contact.Url,
                },
                License = new()
                {
                    Name = openApiInfo.License.Name,
                    Url = openApiInfo.License.Url,
                },
            };

            return Task.CompletedTask;
        });

        return options;
    }

    public static OpenApiOptions ApplySecuritySchemeDefinitions(this OpenApiOptions options, string[] scopes)
    {
        options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();

        options.AddOperationTransformer((operation, context, cancellationToken) =>
        {
            var jwtBarerScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            };

            operation.Security =
            [
                new()
                {
                    [jwtBarerScheme] = scopes
                }
            ];

            return Task.CompletedTask;
        });

        return options;
    }

    public static OpenApiOptions ApplyOperationDeprecatedStatus(this OpenApiOptions options)
    {
        options.AddOperationTransformer((operation, context, cancellationToken) =>
        {
            var apiDescription = context.Description;
            operation.Deprecated |= apiDescription.IsDeprecated();
            return Task.CompletedTask;
        });
        return options;
    }

    public sealed class BearerSecuritySchemeTransformer(
        IConfiguration configuration,
        IAuthenticationSchemeProvider authenticationSchemeProvider)
        : IOpenApiDocumentTransformer
    {
        public async Task TransformAsync(
            OpenApiDocument document,
            OpenApiDocumentTransformerContext context,
            CancellationToken cancellationToken)
        {
            var identitySettings = configuration.GetSection(IdentityProviderSettings.SectionName)
                .Get<IdentityProviderSettings>();

            if (identitySettings is null)
            {
                return;
            }

            var enabledProvider = identitySettings.EnabledProviderName;

            if (enabledProvider is null)
            {
                return;
            }

            var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();
            if (authenticationSchemes.Any(scheme => scheme.Name == JwtBearerDefaults.AuthenticationScheme))
            {
                var requirements = new Dictionary<string, OpenApiSecurityScheme>
                {
                    ["Bearer"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = JwtBearerDefaults.AuthenticationScheme,
                        In = ParameterLocation.Header,
                        BearerFormat = "JSON Web Token",
                        Reference = new OpenApiReference
                        {
                            Id = JwtBearerDefaults.AuthenticationScheme,
                            Type = ReferenceType.SecurityScheme,
                        },
                        Description = "JWT Authorization: Bearer {token}",
                    }
                };

                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes = requirements;
            }
        }
    }

}


