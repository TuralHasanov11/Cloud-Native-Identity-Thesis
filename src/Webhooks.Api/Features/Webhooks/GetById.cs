using System.Security.Claims;

namespace Webhooks.Api.Features.Webhooks;

public static class GetById
{
    public static async Task<Results<Ok<WebhookSubscriptionDto>, NotFound<string>>> Handle(
        IWebhookSubscriptionRepository webhookSubscriptionRepository,
        ClaimsPrincipal user,
        Guid id,
        CancellationToken cancellationToken)
    {
        var userId = user.GetUserId();
        var subscription = await webhookSubscriptionRepository.SingleOrDefaultAsync(
            new WebhookSubscriptionSpecification(new IdentityId(userId), new WebhookId(id)),
            cancellationToken);

        if (subscription is null)
        {
            return TypedResults.NotFound($"Subscriptions {id} not found");
        }

        return TypedResults.Ok(subscription.ToWebhookSubscriptionDto());
    }
}
