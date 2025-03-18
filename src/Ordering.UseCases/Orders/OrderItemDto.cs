namespace Ordering.UseCases.Orders;

public record OrderItemDto(Guid ProductId, string ProductName, Uri PictureUrl, decimal UnitPrice, int Units);
