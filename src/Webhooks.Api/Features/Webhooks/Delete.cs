using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Webhooks.UseCases.Webhooks.Commands;

namespace Webhooks.Api.Features.Webhooks;

public static class Delete
{
    public static async Task<Results<Accepted, NotFound<string>>> Handle(
        IMediator mediator,
        ClaimsPrincipal user,
        Guid id)
    {
        var result = await mediator.Send(new DeleteWebhookSubscriptionCommand(
            user.GetUserId(),
            id));

        if (result.IsSuccess)
        {
            return TypedResults.Accepted(new Uri($"/api/webhooks/{result.Value}", UriKind.Relative));
        }
        else
        {
            return TypedResults.NotFound(result.Errors.First());
        }
    }
}
