using ServiceDefaults.Identity;

namespace Ordering.Api.Features;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapOrdersApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/orders");

        api.MapPost("", Orders.Create.Handle)
            .WithName("CreateOrder")
            .WithSummary("Creates a new order")
            .WithDescription("Creates a new order.")
            .WithTags("Orders");

        api.MapPut("cancel", Orders.Cancel.Handle)
            .RequireAuthorization("RoleOrderAdmins")
            .WithName("CancelOrder")
            .WithSummary("Cancels an order")
            .WithDescription("Cancels an order.")
            .WithTags("Orders");

        api.MapPut("ship", Orders.Ship.Handle)
            //.RequireAuthorization("RoleOrderAdmins")
            .WithMetadata(new GroupRequirementAttribute(AwsCognitoGroups.Admins))
            .WithName("ShipOrder")
            .WithSummary("Ships an order")
            .WithDescription("Ships an order.")
            .WithTags("Orders");

        api.MapGet("{id:guid}", Orders.GetById.Handle)
            .WithName("GetOrderById")
            .WithSummary("Gets an order by ID")
            .WithDescription("Gets an order by its unique identifier.")
            .WithTags("Orders");

        api.MapGet("user", Orders.ListByUser.Handle)
            .WithName("ListOrdersByUser")
            .WithSummary("Lists orders by user")
            .WithDescription("Lists orders.")
            .WithTags("Orders");

        api.MapGet("card-types", CardTypes.List.Handle)
            .WithName("ListCardTypes")
            .WithSummary("Lists card types")
            .WithDescription("Lists available card types.")
            .WithTags("Card Types");

        api.MapPost("draft", Orders.Draft.Handle)
            .WithName("DraftOrder")
            .WithSummary("Creates a draft order")
            .WithDescription("Creates a draft order.")
            .WithTags("Orders");

        return app;
    }
}
