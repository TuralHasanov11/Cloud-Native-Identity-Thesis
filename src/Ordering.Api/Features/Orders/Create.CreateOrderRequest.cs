using Ordering.UseCases.Orders;

namespace Ordering.Api.Features.Orders;

public sealed record CreateOrderRequest(
    Guid UserId,
    string UserName,
    string City,
    string Street,
    string State,
    string Country,
    string ZipCode,
    string CardNumber,
    string CardHolderName,
    DateTime CardExpiration,
    string CardSecurityNumber,
    int CardTypeId,
    Guid Customer,
    IReadOnlyCollection<BasketItemDto> Items);
