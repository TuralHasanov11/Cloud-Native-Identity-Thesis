using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Audit;

public static class AuditEntryExtensions
{
    public static void UseAudit(this ModelBuilder builder)
    {
        builder.Entity<AuditEntry>(b =>
        {
            b.ToTable("audit_entries");

            b.HasKey(x => x.Id);

            b.Property(x => x.Metadata)
                .IsRequired();

            b.Property(x => x.StartTimeUtc)
                .IsRequired();

            b.Property(x => x.EndTimeUtc)
                .IsRequired();

            b.Property(x => x.Succeeded)
                .IsRequired();

            b.Property(x => x.ErrorMessage)
                .HasMaxLength(255)
                .IsRequired(false);

            b.HasIndex(x => x.StartTimeUtc);
        });
    }

    public static void AddAudit(this IHostApplicationBuilder builder)
    {
        builder.Services.AddKeyedScoped<ICollection<AuditEntry>>("Audit", (_, _) => []);
    }

    public static IInterceptor GetAuditInterceptor(
        this IHostApplicationBuilder _,
        IServiceProvider serviceProvider)
    {
        return new AuditInterceptor(
                serviceProvider.GetRequiredKeyedService<ICollection<AuditEntry>>("Audit"),
                serviceProvider.GetRequiredService<IPublishEndpoint>());
    }

    public static void AddRange(this ICollection<AuditEntry> source, ICollection<AuditEntry> entries)
    {
        foreach (var entry in entries)
        {
            source.Add(entry);
        }
    }
}
