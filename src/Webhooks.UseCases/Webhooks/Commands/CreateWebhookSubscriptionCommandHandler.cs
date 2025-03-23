using Ardalis.Result;
using SharedKernel;

namespace Webhooks.UseCases.Webhooks.Commands;

public sealed class CreateWebhookSubscriptionCommandHandler(
    IGrantUrlTesterService grantUrlTesterService,
    IUnitOfWork unitOfWork,
    IWebhookRepository webhookRepository)
    : ICommandHandler<CreateWebhookSubscriptionCommand, WebhookSubscriptionDto>
{
    public async Task<Result<WebhookSubscriptionDto>> Handle(
        CreateWebhookSubscriptionCommand request,
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
                request.UserId);

            await webhookRepository.CreateAsync(subscription, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(subscription.ToWebhookSubscriptionDto());
        }

        return Result.Forbidden("Invalid grant URL");
    }
}
