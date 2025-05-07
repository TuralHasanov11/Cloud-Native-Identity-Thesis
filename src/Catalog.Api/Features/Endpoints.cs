namespace Catalog.Api.Features;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapCatalogApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api");

        api.MapGet("products", Products.List.Handle)
            .AllowAnonymous()
            .WithName("ListProducts")
            .WithSummary("List catalog products")
            .WithDescription("Get a paginated list of products in the catalog.")
            .WithTags("Products");

        api.MapGet("products/by", Products.ListByIds.Handle)
            .AllowAnonymous()
            .WithName("ListProductsByIds")
            .WithSummary("Batch get catalog products")
            .WithDescription("Get multiple products from the catalog")
            .WithTags("Products");

        api.MapGet("products/{id:guid}", Products.GetById.Handle)
            .AllowAnonymous()
            .WithName("GetProductById")
            .WithSummary("Get catalog product")
            .WithDescription("Get an product from the catalog")
            .WithTags("Products");

        api.MapGet("products/by/{name:minlength(1)}", Products.ListByName.Handle)
            .AllowAnonymous()
            .WithName("ListProductsByName")
            .WithSummary("Get catalog products by name")
            .WithDescription("Get a paginated list of catalog products with the specified name.")
            .WithTags("Products");

        ////api.MapGet("/products/{id:int}/pic", GetItemPictureById)
        ////    .WithName("GetItemPicture")
        ////    .WithSummary("Get catalog product picture")
        ////    .WithDescription("Get the picture for a catalog product")
        ////    .WithTags("Items");

        //// Routes for resolving catalog products using AI.
        ////v1.MapGet("/products/withsemanticrelevance/{text:minlength(1)}", GetItemsBySemanticRelevanceV1)
        ////    .WithName("GetRelevantItems")
        ////    .WithSummary("Search catalog for relevant products")
        ////    .WithDescription("Search the catalog for products related to the specified text")
        ////    .WithTags("Search");

        //// Routes for resolving catalog products using AI.
        ////v2.MapGet("/products/withsemanticrelevance", GetItemsBySemanticRelevance)
        ////    .WithName("GetRelevantItems-V2")
        ////    .WithSummary("Search catalog for relevant products")
        ////    .WithDescription("Search the catalog for products related to the specified text")
        ////    .WithTags("Search");

        //// Routes for resolving catalog products by type and brand.
        ////v1.MapGet("/products/type/{typeId}/brand/{brandId?}", GetItemsByBrandAndTypeId)
        ////    .WithName("GetItemsByTypeAndBrand")
        ////    .WithSummary("Get catalog products by type and brand")
        ////    .WithDescription("Get catalog products of the specified type and brand")
        ////    .WithTags("Types");

        ////v1.MapGet("/products/type/all/brand/{brandId:int?}", GetItemsByBrandId)
        ////    .WithName("GetItemsByBrand")
        ////    .WithSummary("List catalog products by brand")
        ////    .WithDescription("Get a list of catalog products for the specified brand")
        ////    .WithTags("Brands");

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
            .RequireAuthorization("RequireGroupAdmins")
            .WithName("CreateProduct")
            .WithSummary("Create a catalog product")
            .WithDescription("Create a new product in the catalog")
            .WithTags("Products");

        api.MapPut("products/{id:guid}", Products.Update.Handle)
            .RequireAuthorization("RequireGroupAdmins")
            .WithName("UpdateProduct")
            .WithSummary("Create or replace a catalog product")
            .WithDescription("Create or replace a catalog product")
            .WithTags("Products");

        api.MapDelete("products/{id:guid}", Products.DeleteById.Handle)
            .RequireAuthorization("RequireGroupAdmins")
            .WithName("DeleteProductById")
            .WithSummary("Delete catalog product")
            .WithDescription("Delete the specified catalog product")
            .WithTags("Products");

        return app;
    }
}
