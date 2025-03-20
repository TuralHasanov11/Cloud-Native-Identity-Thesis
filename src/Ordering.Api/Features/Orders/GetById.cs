using Microsoft.AspNetCore.Http.HttpResults;
using Ordering.UseCases.Orders.Queries;

namespace Ordering.Api.Features.Orders;

public static class GetById
{
    public static async Task<Results<Ok<OrderDto>, NotFound>> Handle(
        IMediator mediator,
        Guid orderId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetOrderByIdQuery(orderId), cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }

        return TypedResults.NotFound();
    }
}
