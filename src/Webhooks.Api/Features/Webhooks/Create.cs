using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Webhooks.UseCases.Webhooks.Commands;

namespace Webhooks.Api.Features.Webhooks;

public static class Create
{
    public static async Task<Results<Created, BadRequest<string>>> Handle(
        IMediator mediator,
        ClaimsPrincipal user,
        CreateWebhookSubscriptionRequest request)
    {
        var result = await mediator.Send(new CreateWebhookSubscriptionCommand(
            request.Url,
            request.Token,
            request.Event,
            request.GrantUrl,
            user.GetUserId()));

        if (result.IsSuccess)
        {
            return TypedResults.Created(new Uri($"/api/webhooks/{result.Value.Id}", UriKind.Relative));
        }
        else
        {
            return TypedResults.BadRequest($"Invalid grant URL: {request.GrantUrl}");
        }
    }
}
