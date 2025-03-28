namespace Ordering.Api.Features.Orders;

public record OrderItemDto(
    Guid ProductId,
    string ProductName,
    Uri PictureUrl,
    decimal UnitPrice,
    int Units,
    decimal Discount = 0);
