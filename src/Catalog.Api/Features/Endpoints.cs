using ServiceDefaults.Identity;

namespace Catalog.Api.Features;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapCatalogApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api");

        const string productsTag = "Products";

        api.MapGet("products", Products.List.Handle)
            .AllowAnonymous()
            .WithName("ListProducts")
            .WithSummary("List catalog products")
            .WithDescription("Get a list of products in the catalog.")
            .WithTags(productsTag);

        api.MapGet("products/by", Products.ListByIds.Handle)
            .AllowAnonymous()
            .WithName("ListProductsByIds")
            .WithSummary("Batch get catalog products")
            .WithDescription("Get multiple products from the catalog")
            .WithTags(productsTag);

        api.MapGet("products/{id:guid}", Products.GetById.Handle)
            .AllowAnonymous()
            .WithName("GetProductById")
            .WithSummary("Get catalog product")
            .WithDescription("Get an product from the catalog")
            .WithTags(productsTag);

        api.MapGet("products/by/{name:minlength(1)}", Products.ListByName.Handle)
            .AllowAnonymous()
            .WithName("ListProductsByName")
            .WithSummary("Get catalog products by name")
            .WithDescription("Get a list of catalog products with the specified name.")
            .WithTags(productsTag);

        api.MapGet("product-types", ProductTypes.List.Handle)
            .AllowAnonymous()
            .WithName("ListProductTypes")
            .WithSummary("List catalog product types")
            .WithDescription("Get a list of the types of catalog products")
            .WithTags("Product Types");

        api.MapGet("brands", Brands.List.Handle)
            .AllowAnonymous()
            .WithName("ListBrands")
            .WithSummary("List catalog product brands")
            .WithDescription("Get a list of the brands of catalog products")
            .WithTags("Brands");

        api.MapPost("products", Products.Create.Handle)
            .WithMetadata(new GroupRequirementAttribute(AwsCognitoGroups.Admins))
            //.RequireAuthorization("RoleCatalogAdmins")
            .WithName("CreateProduct")
            .WithSummary("Create a catalog product")
            .WithDescription("Create a new product in the catalog")
            .WithTags(productsTag);

        api.MapPut("products/{id:guid}", Products.Update.Handle)
            .WithMetadata(new GroupRequirementAttribute(AwsCognitoGroups.Admins))
            //.RequireAuthorization("RoleCatalogAdmins")
            .WithName("UpdateProduct")
            .WithSummary("Create or replace a catalog product")
            .WithDescription("Create or replace a catalog product")
            .WithTags(productsTag);

        api.MapDelete("products/{id:guid}", Products.DeleteById.Handle)
            .WithMetadata(new GroupRequirementAttribute(AwsCognitoGroups.Admins))
            //.RequireAuthorization("RoleCatalogAdmins")
            .WithName("DeleteProductById")
            .WithSummary("Delete catalog product")
            .WithDescription("Delete the specified catalog product")
            .WithTags(productsTag);

        return app;
    }
}
