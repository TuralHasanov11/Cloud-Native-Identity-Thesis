using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Webhooks.UseCases.Webhooks;
using Webhooks.UseCases.Webhooks.Queries;

namespace Webhooks.Api.Features.Webhooks;

public static class List
{
    public static async Task<Ok<IEnumerable<WebhookSubscriptionDto>>> Handle(
        IMediator mediator, ClaimsPrincipal user)
    {
        var userId = user.GetUserId();

        var result = await mediator.Send(new ListWebhookSubscriptionsQuery(userId));

        return TypedResults.Ok(result.Value);
    }
}
