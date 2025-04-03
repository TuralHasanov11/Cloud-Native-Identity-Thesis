using System.Security.Claims;

namespace Webhooks.Api.Features.Webhooks;

public static class Delete
{
    public static async Task<Results<Accepted, NotFound<string>>> Handle(
        IWebhookSubscriptionRepository webhookSubscriptionRepository,
        ClaimsPrincipal user,
        Guid id,
        CancellationToken cancellationToken)
    {
        var subscription = await webhookSubscriptionRepository.SingleOrDefaultAsync(
            new GetWebhookSubscriptionSpecification(new IdentityId(user.GetUserId()), new WebhookId(id)),
            cancellationToken);

        if (subscription is null)
        {
            return TypedResults.NotFound($"Subscriptions {id} not found");
        }

        webhookSubscriptionRepository.Delete(subscription);

        await webhookSubscriptionRepository.SaveChangesAsync(cancellationToken);

        return TypedResults.Accepted(new Uri($"/api/webhooks/{subscription.Id}", UriKind.Relative));
    }
}
