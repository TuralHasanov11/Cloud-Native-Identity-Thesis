using System.Security.Claims;

namespace Webhooks.Api.Features.Webhooks;

public static class Create
{
    public static async Task<Results<Created, BadRequest<string>>> Handle(
        IWebhookSubscriptionRepository webhookSubscriptionRepository,
        IGrantUrlTesterService grantUrlTesterService,
        ClaimsPrincipal user,
        CreateWebhookSubscriptionRequest request,
        CancellationToken cancellationToken)
    {
        var grantOk = await grantUrlTesterService.TestGrantUrl(
            new Uri(request.Url, UriKind.Absolute),
            new Uri(request.GrantUrl, UriKind.Absolute),
            request.Token ?? string.Empty);

        if (grantOk)
        {
            var subscription = new WebhookSubscription(
                Enum.Parse<WebhookType>(request.Event, ignoreCase: true),
                DateTime.UtcNow,
                new Uri(request.Url, UriKind.Absolute),
                request.Token,
                new IdentityId(user.GetUserId()));

            await webhookSubscriptionRepository.CreateAsync(subscription, cancellationToken);
            await webhookSubscriptionRepository.SaveChangesAsync(cancellationToken);

            return TypedResults.Created(new Uri($"/api/webhooks/{subscription.Id}", UriKind.Relative));
        }

        return TypedResults.BadRequest($"Invalid grant URL: {request.GrantUrl}");
    }
}
