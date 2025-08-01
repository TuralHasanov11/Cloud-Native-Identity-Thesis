﻿namespace Webhooks.Client.Services;

public class WebhookSubscriptionRequest
{
    public string? Url { get; set; }

    public string? Token { get; set; }

    public string? Event { get; set; }

    public string? GrantUrl { get; set; }
}
