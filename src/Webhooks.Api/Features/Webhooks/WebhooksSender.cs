using System.Text;
using System.Text.Json;
using Webhooks.Infrastructure.Services;

namespace Webhooks.Api.Features.Webhooks;

public class WebhooksSender(IHttpClientFactory httpClientFactory, ILogger<WebhooksSender> logger) : IWebhooksSender
{
    public async Task SendAll(IEnumerable<WebhookSubscription> receivers, WebhookData data)
    {
        using var client = httpClientFactory.CreateClient();
        var json = JsonSerializer.Serialize(data);
        var tasks = receivers.Select(r => OnSendData(r, json, client));
        await Task.WhenAll(tasks);
    }

    private Task OnSendData(WebhookSubscription subs, string jsonData, HttpClient client)
    {
        using var request = new HttpRequestMessage()
        {
            RequestUri = subs.DestinationUrl,
            Method = HttpMethod.Post,
            Content = new StringContent(jsonData, Encoding.UTF8, "application/json")
        };

        if (!string.IsNullOrWhiteSpace(subs.Token))
        {
            request.Headers.Add("X-eshop-whtoken", subs.Token);
        }

        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("Sending hook to {DestUrl} of type {Type}", subs.DestinationUrl, subs.Type);
        }

        return client.SendAsync(request);
    }

}
