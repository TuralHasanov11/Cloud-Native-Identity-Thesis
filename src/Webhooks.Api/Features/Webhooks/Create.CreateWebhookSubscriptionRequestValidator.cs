using FluentValidation;
using Webhooks.Core.WebhookAggregate;

namespace Webhooks.Api.Features.Webhooks;

public class CreateWebhookSubscriptionRequestValidator : AbstractValidator<CreateWebhookSubscriptionRequest>
{
    public CreateWebhookSubscriptionRequestValidator()
    {
        RuleFor(x => x.Url)
            .NotEmpty()
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            .WithMessage("Url must be a well-formed absolute URL.");

        RuleFor(x => x.Token).NotEmpty();

        RuleFor(x => x.Event)
            .NotEmpty()
            .Must(eventType => Enum.TryParse<WebhookType>(eventType, out _))
            .WithMessage("Event must be a valid WebhookType.");

        RuleFor(x => x.GrantUrl)
            .NotEmpty()
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            .WithMessage("GrantUrl must be a well-formed absolute URL.");
    }
}
