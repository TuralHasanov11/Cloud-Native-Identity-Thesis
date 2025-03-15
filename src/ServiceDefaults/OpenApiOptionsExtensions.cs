using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace ServiceDefaults;

internal static class OpenApiOptionsExtensions
{
    public static OpenApiOptions ApplyApiVersionInfo(this OpenApiOptions options, OpenApiInfo openApiInfo)
    {
        options.AddDocumentTransformer((document, context, cancellationToken) =>
        {
            //var versionedDescriptionProvider = context.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
            //var apiDescription = versionedDescriptionProvider?.ApiVersionDescriptions
            //    .SingleOrDefault(description => description.GroupName == context.DocumentName);
            //if (apiDescription is null)
            //{
            //    return Task.CompletedTask;
            //}
            //document.Info.Version = apiDescription.ApiVersion.ToString();
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

    //public static OpenApiOptions ApplySecuritySchemeDefinitions(this OpenApiOptions options)
    //{
    //    options.AddDocumentTransformer<SecuritySchemeDefinitionsTransformer>();
    //    return options;
    //}

    //public static OpenApiOptions ApplyAuthorizationChecks(this OpenApiOptions options, string[] scopes)
    //{
    //    options.AddOperationTransformer((operation, context, cancellationToken) =>
    //    {
    //        var metadata = context.Description.ActionDescriptor.EndpointMetadata;

    //        if (!metadata.OfType<IAuthorizeData>().Any())
    //        {
    //            return Task.CompletedTask;
    //        }

    //        operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
    //        operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

    //        var oAuthScheme = new OpenApiSecurityScheme
    //        {
    //            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
    //        };

    //        operation.Security = new List<OpenApiSecurityRequirement>
    //        {
    //            new()
    //            {
    //                [oAuthScheme] = scopes
    //            }
    //        };

    //        return Task.CompletedTask;
    //    });
    //    return options;
    //}

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

    //public static OpenApiOptions ApplyApiVersionDescription(this OpenApiOptions options)
    //{
    //    options.AddOperationTransformer((operation, context, cancellationToken) =>
    //    {
    //        // Find parameter named "api-version" and add a description to it
    //        var apiVersionParameter = operation.Parameters.FirstOrDefault(p => p.Name == "api-version");
    //        if (apiVersionParameter is not null)
    //        {
    //            apiVersionParameter.Description = "The API version, in the format 'major.minor'.";
    //            switch (context.DocumentName)
    //            {
    //                case "v1":
    //                    apiVersionParameter.Schema.Example = new OpenApiString("1.0");
    //                    break;
    //                case "v2":
    //                    apiVersionParameter.Schema.Example = new OpenApiString("2.0");
    //                    break;
    //            }
    //        }
    //        return Task.CompletedTask;
    //    });
    //    return options;
    //}

    //// This extension method adds a schema transformer that sets "nullable" to false for all optional properties.
    //public static OpenApiOptions ApplySchemaNullableFalse(this OpenApiOptions options)
    //{
    //    options.AddSchemaTransformer((schema, context, cancellationToken) =>
    //    {
    //        if (schema.Properties is not null)
    //        {
    //            foreach (var property in schema.Properties)
    //            {
    //                if (schema.Required?.Contains(property.Key) != true)
    //                {
    //                    property.Value.Nullable = false;
    //                }
    //            }
    //        }

    //        return Task.CompletedTask;
    //    });
    //    return options;
    //}

    //private class SecuritySchemeDefinitionsTransformer(IConfiguration configuration) : IOpenApiDocumentTransformer
    //{
    //    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    //    {
    //        var identitySection = configuration.GetSection("Identity");
    //        if (!identitySection.Exists())
    //        {
    //            return Task.CompletedTask;
    //        }

    //        var identityUrlExternal = identitySection.GetRequiredValue("Url");
    //        var scopes = identitySection.GetRequiredSection("Scopes").GetChildren().ToDictionary(p => p.Key, p => p.Value);
    //        var securityScheme = new OpenApiSecurityScheme
    //        {
    //            Type = SecuritySchemeType.OAuth2,
    //            Flows = new OpenApiOAuthFlows()
    //            {
    //                // TODO: Change this to use Authorization Code flow with PKCE
    //                Implicit = new OpenApiOAuthFlow()
    //                {
    //                    AuthorizationUrl = new Uri($"{identityUrlExternal}/connect/authorize"),
    //                    TokenUrl = new Uri($"{identityUrlExternal}/connect/token"),
    //                    Scopes = scopes,
    //                }
    //            }
    //        };
    //        document.Components ??= new();
    //        document.Components.SecuritySchemes.Add("oauth2", securityScheme);
    //        return Task.CompletedTask;
    //    }
    //}
}
