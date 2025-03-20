using FluentValidation;

namespace Ordering.Api.Features.Orders;

public sealed class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.City).NotEmpty();
        RuleFor(x => x.Street).NotEmpty();
        RuleFor(x => x.State).NotEmpty();
        RuleFor(x => x.Country).NotEmpty();
        RuleFor(x => x.ZipCode).NotEmpty();
        RuleFor(x => x.CardNumber).NotEmpty();
        RuleFor(x => x.CardHolderName).NotEmpty();
        RuleFor(x => x.CardExpiration).NotEmpty();
        RuleFor(x => x.CardSecurityNumber).NotEmpty();
        RuleFor(x => x.CardTypeId).NotEmpty();
        RuleFor(x => x.Customer).NotEmpty();
        RuleFor(x => x.Items).NotEmpty();
    }
}
