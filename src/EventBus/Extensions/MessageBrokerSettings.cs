using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace EventBus.Extensions;

public class MessageBrokerSettings
{
    public const string SectionName = "MessageBroker";

    [Required]
    public string Host { get; set; } = string.Empty;

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

[OptionsValidator]
public partial class ValidateMessageBrokerSettings : IValidateOptions<MessageBrokerSettings>;
