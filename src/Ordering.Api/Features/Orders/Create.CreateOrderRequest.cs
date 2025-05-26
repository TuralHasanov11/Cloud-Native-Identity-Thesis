namespace Ordering.Api.Features.Orders;

public sealed record CreateOrderRequest(
    string UserName,
    string City,
    string Street,
    string State,
    string Country,
    string ZipCode,
    int CardTypeId,
    IReadOnlyCollection<BasketItemDto> Items);
