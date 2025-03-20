using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ordering.Api.Features.Orders;

public static class Create
{
    public static async Task<Results<Ok, ProblemHttpResult>> Handle(
        IMediator mediator,
        CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        var maskedCCNumber = request.CardNumber.Substring(request.CardNumber.Length - 4).PadLeft(request.CardNumber.Length, 'X');
        var createOrderCommand = new CreateOrderCommand(
            request.Items.ToOrderItemsDto(),
            request.UserId,
            request.UserName,
            request.City,
            request.Street,
            request.State,
            request.Country,
            request.ZipCode,
            maskedCCNumber,
            request.CardHolderName,
            request.CardExpiration,
            request.CardSecurityNumber,
            request.CardTypeId);

        var result = await mediator.Send(createOrderCommand, cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok();
        }

        return TypedResults.Problem(
            new ProblemDetails()
            {
                Title = "Order creation failed",
                Detail = result.Errors.First()
            });
    }
}
