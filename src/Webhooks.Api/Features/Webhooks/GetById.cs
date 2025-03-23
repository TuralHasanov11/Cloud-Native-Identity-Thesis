using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Webhooks.UseCases.Webhooks;
using Webhooks.UseCases.Webhooks.Queries;

namespace Webhooks.Api.Features.Webhooks;

public static class GetById
{
    public static async Task<Results<Ok<WebhookSubscriptionDto>, NotFound<string>>> Handle(
        IMediator mediator, ClaimsPrincipal user, Guid id)
    {
        var userId = user.GetUserId();
        var result = await mediator.Send(new GetWebhookSubscriptionQuery(userId, id));

        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }
        return TypedResults.NotFound($"Subscriptions {id} not found");
    }
}
