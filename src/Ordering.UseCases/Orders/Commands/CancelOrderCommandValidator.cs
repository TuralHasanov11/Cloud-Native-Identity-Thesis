using FluentValidation;

namespace Ordering.UseCases.Orders.Commands;

public sealed class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator()
    {
        RuleFor(order => order.OrderNumber).NotEmpty().WithMessage("No {PropertyName} found");
    }
}
