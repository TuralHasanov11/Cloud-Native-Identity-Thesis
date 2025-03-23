using SharedKernel;

namespace Webhooks.Core.WebhookAggregate.Specifications;

public class GetWebhookSubscriptionSpecification(IdentityId UserId, WebhookId Id)
    : Specification<WebhookSubscription>(ws => ws.UserId == UserId && ws.Id == Id);
