using SharedKernel;

namespace Webhooks.Core.WebhookAggregate.Specifications;

public class GetWebhookSubscriptionsSpecification(IdentityId UserId)
    : Specification<WebhookSubscription>(ws => ws.UserId == UserId);
