namespace Ordering.UseCases.Orders.Commands;

public sealed record CreateOrderCommand(
    IReadOnlyCollection<OrderItemDto> OrderItems,
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
    int CardTypeId) : ICommand<bool>;
