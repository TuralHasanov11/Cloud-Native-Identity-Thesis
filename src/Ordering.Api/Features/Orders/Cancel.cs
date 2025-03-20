using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ordering.Api.Features.Orders;

public static class Cancel
{
    public static async Task<Results<Ok, ProblemHttpResult>> Handle(
        IMediator mediator,
        CancelOrderRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CancelOrderCommand(request.OrderNumber), cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok();
        }

        return TypedResults.Problem(
            new ProblemDetails()
            {
                Title = "Order cancellation failed",
                Detail = result.Errors.First()
            });
    }
}
