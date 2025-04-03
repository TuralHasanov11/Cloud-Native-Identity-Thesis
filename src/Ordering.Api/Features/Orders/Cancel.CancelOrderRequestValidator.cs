namespace Ordering.Api.Features.Orders;

public sealed class CancelOrderRequestValidator : AbstractValidator<CancelOrderRequest>
{
    public CancelOrderRequestValidator()
    {
        RuleFor(x => x.OrderNumber).NotEmpty();
    }
}
