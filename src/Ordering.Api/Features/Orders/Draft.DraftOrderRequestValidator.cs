namespace Ordering.Api.Features.Orders;

public class DraftOrderRequestValidator : AbstractValidator<DraftOrderRequest>
{
    public DraftOrderRequestValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty();
        RuleFor(x => x.Items)
            .NotEmpty();
    }
}
