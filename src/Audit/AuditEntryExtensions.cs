using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Audit;

public static class AuditEntryExtensions
{
    public static void UseAudit(this ModelBuilder builder)
    {
        builder.Entity<AuditEntry>(builder =>
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Metadata)
                .IsRequired();

            builder.Property(x => x.StartTimeUtc)
                .IsRequired();

            builder.Property(x => x.EndTimeUtc)
                .IsRequired();

            builder.Property(x => x.Succeeded)
                .IsRequired();

            builder.Property(x => x.ErrorMessage)
                .HasMaxLength(255)
                .IsRequired(false);

            builder.HasIndex(x => x.StartTimeUtc);
        });
    }

    public static void AddAudit(this IHostApplicationBuilder builder)
    {
        builder.Services.AddKeyedScoped<List<AuditEntry>>("Audit", (_, _) => []);
    }

    public static IInterceptor GetAuditInterceptor(
        this IHostApplicationBuilder _,
        IServiceProvider serviceProvider)
    {
        return new AuditInterceptor(
                serviceProvider.GetRequiredKeyedService<List<AuditEntry>>("Audit"),
                serviceProvider.GetRequiredService<IPublishEndpoint>());
    }
}
