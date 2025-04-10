namespace Ordering.Api.Features.Orders;

public sealed record CreateOrderRequest(
    string UserId,
    string UserName,
    string City,
    string Street,
    string State,
    string Country,
    string ZipCode,
    int CardTypeId,
    Guid Customer,
    IReadOnlyCollection<BasketItemDto> Items);
