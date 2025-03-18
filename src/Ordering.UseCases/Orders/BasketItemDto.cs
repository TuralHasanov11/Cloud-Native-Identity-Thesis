namespace Ordering.UseCases.Orders;

public record BasketItemDto(
    string Id,
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    decimal OldUnitPrice,
    int Quantity,
    Uri PictureUrl);
