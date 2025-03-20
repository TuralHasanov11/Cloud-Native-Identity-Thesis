using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ordering.Api.Features.Orders;

public static class Ship
{
    public static async Task<Results<Ok, ProblemHttpResult>> Handle(
        IMediator mediator,
        ShipOrderRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ShipOrderCommand(request.OrderNumber), cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok();
        }

        return TypedResults.Problem(
            new ProblemDetails()
            {
                Title = "Ship order failed to process",
                Detail = result.Errors.First()
            });
    }
}
