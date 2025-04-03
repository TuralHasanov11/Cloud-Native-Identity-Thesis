namespace Webhooks.Core.WebhookAggregate;

public interface IGrantUrlTesterService
{
    Task<bool> TestGrantUrl(Uri urlHook, Uri url, string token);
}
