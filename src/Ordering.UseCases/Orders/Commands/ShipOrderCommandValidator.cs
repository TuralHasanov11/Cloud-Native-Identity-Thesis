using FluentValidation;

namespace Ordering.UseCases.Orders.Commands;

public sealed class ShipOrderCommandValidator : AbstractValidator<ShipOrderCommand>
{
    public ShipOrderCommandValidator()
    {
        RuleFor(order => order.OrderNumber).NotEmpty().WithMessage("No {PropertyName} found");
    }
}
