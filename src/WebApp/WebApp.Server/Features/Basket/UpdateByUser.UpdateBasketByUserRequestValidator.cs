using FluentValidation;

namespace WebApp.Server.Features.Basket;

public sealed class CreateOrderRequestValidator : AbstractValidator<UpdateBasketByUserRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(request => request.Items)
            .NotEmpty()
            .WithMessage("Basket items cannot be empty.");

        RuleForEach(request => request.Items)
            .SetValidator(new BasketQuantityValidator());
    }
}

public sealed class BasketQuantityValidator : AbstractValidator<BasketQuantity>
{
    public BasketQuantityValidator()
    {
        RuleFor(item => item.ProductId)
            .NotEmpty()
            .WithMessage("Product Id cannot be empty.");
        RuleFor(item => item.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");
    }
}
