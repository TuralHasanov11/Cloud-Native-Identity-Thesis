namespace Ordering.Api.Features.Orders;

public sealed class ShipOrderRequestValidator : AbstractValidator<ShipOrderRequest>
{
    public ShipOrderRequestValidator()
    {
        RuleFor(x => x.OrderNumber).NotEmpty();
    }
}
