namespace Ordering.Api.Features.Orders;
public record CustomerBasketDto(Guid CustomerId, IReadOnlyCollection<BasketItemDto> Items);
