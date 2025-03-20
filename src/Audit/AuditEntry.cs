namespace Audit;

public class AuditEntry
{
    public Guid Id { get; set; }

    public string Metadata { get; set; } = string.Empty;

    public DateTime StartTimeUtc { get; set; }

    public DateTime EndTimeUtc { get; set; }

    public bool Succeeded { get; set; }

    public string? ErrorMessage { get; set; }
}

public record AuditTrailMessage(ICollection<AuditEntry> Entries);
