using Ardalis.Result;
using Webhooks.Core.WebhookAggregate.Specifications;

namespace Webhooks.UseCases.Webhooks.Commands;

public sealed class DeleteWebhookSubscriptionCommandHandler(
    IWebhookRepository webhookRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteWebhookSubscriptionCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        DeleteWebhookSubscriptionCommand request,
        CancellationToken cancellationToken)
    {
        var subscription = await webhookRepository.SingleOrDefaultAsync(
            new GetWebhookSubscriptionSpecification(new IdentityId(request.UserId), new WebhookId(request.Id)),
            cancellationToken);

        if (subscription is null)
        {
            return Result.NotFound($"Subscriptions {request.Id} not found");
        }

        webhookRepository.Delete(subscription);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success<Guid>(subscription.Id);
    }
}
