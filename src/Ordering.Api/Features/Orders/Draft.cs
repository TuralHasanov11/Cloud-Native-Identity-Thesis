using Microsoft.AspNetCore.Mvc;

namespace Ordering.Api.Features.Orders;

public static class Draft
{
    public static async Task<Results<Ok<OrderDraftDto>, ProblemHttpResult>> Handle(
        IMediator mediator,
        DraftOrderRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateOrderDraftCommand(request.CustomerId, request.Items), cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }

        return TypedResults.Problem(
            new ProblemDetails()
            {
                Title = "Order creation failed",
                Detail = result.Errors.First()
            });
    }
}
