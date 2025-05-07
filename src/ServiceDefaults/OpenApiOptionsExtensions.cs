using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

            document.Info.Version = apiDescription.ApiVersion.ToString();

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

    public static OpenApiOptions ApplySecuritySchemeDefinitions(this OpenApiOptions options)
    {
        options.AddDocumentTransformer<SecuritySchemeDefinitionsTransformer>();

        return options;
    }

    public static OpenApiOptions ApplyAuthorizationChecks(this OpenApiOptions options, string[] scopes)
    {
        options.AddOperationTransformer((operation, context, cancellationToken) =>
        {
            var metadata = context.Description.ActionDescriptor.EndpointMetadata;

            if (!metadata.OfType<IAuthorizeData>().Any())
            {
                return Task.CompletedTask;
            }

            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

            var jwtBarerScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = JwtBearerDefaults.AuthenticationScheme }
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

    public static OpenApiOptions ApplyApiVersionDescription(this OpenApiOptions options)
    {
        //options.AddOperationTransformer((operation, context, cancellationToken) =>
        //{
        //    return Task.CompletedTask;
        //});

        return options;
    }

    public static OpenApiOptions ApplySchemaNullableFalse(this OpenApiOptions options)
    {
        options.AddSchemaTransformer((schema, context, cancellationToken) =>
        {
            if (schema.Properties is not null)
            {
                foreach (var property in schema.Properties)
                {
                    if (schema.Required?.Contains(property.Key) != true)
                    {
                        property.Value.Nullable = false;
                    }
                }
            }

            return Task.CompletedTask;
        });

        return options;
    }

    private sealed class SecuritySchemeDefinitionsTransformer(IConfiguration configuration) : IOpenApiDocumentTransformer
    {
        public Task TransformAsync(
            OpenApiDocument document,
            OpenApiDocumentTransformerContext context,
            CancellationToken cancellationToken)
        {
            var identitySettings = configuration.GetSection(IdentityProviderSettings.SectionName).Get<IdentityProviderSettings>();

            if (identitySettings is null)
            {
                return Task.CompletedTask;
            }

            var enabledProvider = identitySettings.EnabledProviderName;

            if (enabledProvider is null)
            {
                return Task.CompletedTask;
            }

            //string[] scopes = [];

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.OAuth2,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                //Flows = new OpenApiOAuthFlows()
                //{
                //    AuthorizationCode = new OpenApiOAuthFlow()
                //    {
                //        AuthorizationUrl = new Uri($"{enabledProvider.Authority}/connect/authorize"),
                //        TokenUrl = new Uri($"{enabledProvider.Authority}/connect/token"),
                //        Scopes = (IDictionary<string, string>)scopes,
                //    }
                //},
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme,
                },
                In = ParameterLocation.Header,
                BearerFormat = "JSON Web Token",
                Description = "JWT Authorization: Bearer {token}",
            };
            document.Components ??= new();
            document.Components.SecuritySchemes.Add(JwtBearerDefaults.AuthenticationScheme, securityScheme);


            return Task.CompletedTask;
        }
    }
}
