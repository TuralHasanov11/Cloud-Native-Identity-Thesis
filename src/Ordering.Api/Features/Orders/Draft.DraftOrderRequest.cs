namespace Ordering.Api.Features.Orders;

public sealed record DraftOrderRequest(Guid CustomerId, IEnumerable<BasketItemDto> Items);
