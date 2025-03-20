using ServiceDefaults.Identity;

namespace Ordering.Api.Features.Orders;

public static class ListByUser
{
    public static async Task<Ok<IEnumerable<OrderSummary>>> Handle(
        IMediator mediator,
        IIdentityService identityService,
        CancellationToken cancellationToken)
    {
        var user = identityService.GetUser();
        var userId = user.GetUserId();

        var result = await mediator.Send(new ListOrdersByUserQuery(userId), cancellationToken);

        return TypedResults.Ok(result.Value);
    }
}
