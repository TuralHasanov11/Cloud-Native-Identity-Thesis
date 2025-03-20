namespace Ordering.UseCases.Orders;

public record CustomerBasketDto(Guid CustomerId, IReadOnlyCollection<BasketItemDto> Items);
