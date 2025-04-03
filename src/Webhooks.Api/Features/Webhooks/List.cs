using System.Security.Claims;

namespace Webhooks.Api.Features.Webhooks;

public static class List
{
    public static async Task<Ok<IEnumerable<WebhookSubscriptionDto>>> Handle(
        IWebhookSubscriptionRepository webhookSubscriptionRepository,
        ClaimsPrincipal user,
        CancellationToken cancellationToken)
    {
        var userId = user.GetUserId();

        var subscriptions = await webhookSubscriptionRepository.ListAsync(
            new GetWebhookSubscriptionsSpecification(new IdentityId(userId)),
            ws => ws.ToWebhookSubscriptionDto(),
            cancellationToken);

        return TypedResults.Ok(subscriptions);
    }
}
