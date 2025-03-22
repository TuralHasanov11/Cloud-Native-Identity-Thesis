using System.Text.Json.Serialization;

namespace Webhooks.Core.WebhookAggregate;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WebhookType
{
    Default = 0,
    CatalogItemPriceChange = 1,
    OrderShipped = 2,
    OrderPaid = 3
}
