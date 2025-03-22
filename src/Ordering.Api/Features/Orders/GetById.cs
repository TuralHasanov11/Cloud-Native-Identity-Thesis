namespace Ordering.Api.Features.Orders;

public static class GetById
{
    public static async Task<Results<Ok<OrderDto>, NotFound>> Handle(
        IMediator mediator,
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetOrderByIdQuery(id), cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }

        return TypedResults.NotFound();
    }
}
