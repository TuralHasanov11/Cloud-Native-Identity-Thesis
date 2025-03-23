namespace Webhooks.Api.Features;

public static class Endpoints
{
    public static RouteGroupBuilder MapWebhooksApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api/webhooks");

        api.MapGet("", Webhooks.List.Handle)
            .RequireAuthorization()
            .WithName("ListWebhookSubscriptions")
            .WithSummary("Lists webhook subscriptions")
            .WithDescription("Lists webhook subscriptions.")
            .WithTags("WebhookSubscriptions");

        api.MapGet("{id:guid}", Webhooks.GetById.Handle)
            .RequireAuthorization()
            .WithName("GetWebhookSubscriptionById")
            .WithSummary("Gets a webhook subscription")
            .WithDescription("Gets a webhook subscription.")
            .WithTags("WebhookSubscriptions");

        api.MapPost("", Webhooks.Create.Handle)
            .RequireAuthorization()
            .WithName("CreateWebhookSubscription")
            .WithSummary("Creates a webhook subscription")
            .WithDescription("Creates a webhook subscription.")
            .WithTags("WebhookSubscriptions");

        api.MapDelete("{id:guid}", Webhooks.Delete.Handle)
            .RequireAuthorization()
            .WithName("DeleteWebhookSubscription")
            .WithSummary("Deletes a webhook subscription")
            .WithDescription("Deletes a webhook subscription.")
            .WithTags("WebhookSubscriptions");

        return api;
    }

}
