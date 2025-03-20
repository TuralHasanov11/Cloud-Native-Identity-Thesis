using Ordering.UseCases.CardTypes;

namespace Ordering.Api.Features;

public static class Endpoints
{
    public static RouteGroupBuilder MapOrdersApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/orders");

        api.MapPost("/", Orders.Create.Handle)
            .WithName(nameof(Orders.Create))
            .WithSummary("Creates a new order")
            .WithDescription("Creates a new order for the authenticated user.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest, "application/problem+json");

        api.MapPut("/cancel", Orders.Cancel.Handle)
            .WithName(nameof(Orders.Cancel))
            .WithSummary("Cancels an order")
            .WithDescription("Cancels an order for the authenticated user.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest, "application/problem+json");

        api.MapPut("/ship", Orders.Ship.Handle)
            .WithName(nameof(Orders.Ship))
            .WithSummary("Ships an order")
            .WithDescription("Ships an order for the authenticated user.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest, "application/problem+json");

        api.MapGet("{orderId:guid}", Orders.GetById.Handle)
            .WithName(nameof(Orders.GetById))
            .WithSummary("Gets an order by ID")
            .WithDescription("Gets an order by its unique identifier.")
            .Produces<OrderDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound, "application/problem+json");

        api.MapGet("/user", Orders.ListByUser.Handle)
            .WithName(nameof(Orders.ListByUser))
            .WithSummary("Lists orders by user")
            .WithDescription("Lists orders for the authenticated user.")
            .Produces<IEnumerable<OrderSummary>>(StatusCodes.Status200OK);

        api.MapGet("/card-types", CardTypes.List.Handle)
            .WithName(nameof(CardTypes.List))
            .WithSummary("Lists card types")
            .WithDescription("Lists available card types.")
            .Produces<IEnumerable<CardTypeDto>>(StatusCodes.Status200OK);

        api.MapPost("/draft", Orders.Draft.Handle)
            .WithName(nameof(Orders.Draft))
            .WithSummary("Creates a draft order")
            .WithDescription("Creates a draft order for the authenticated user.")
            .Produces<OrderDraftDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest, "application/problem+json");

        return api;
    }
}


