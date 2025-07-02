using ServiceDefaults.Identity;

namespace Ordering.Api.Features.Orders;

public static class ListByUser
{
    public static async Task<Ok<IEnumerable<OrderSummary>>> Handle(
        IOrderRepository orderRepository,
        IIdentityService identityService,
        CancellationToken cancellationToken)
    {
        var user = identityService.GetUser();
        var userId = user?.GetUserId();

        var orders = await orderRepository.ListAsync(
            new OrderSpecification(new IdentityId(userId!)), // TODO: Handle null userId
            o => o.ToOrderSummary(),
            cancellationToken);

        return TypedResults.Ok(orders);
    }
}
