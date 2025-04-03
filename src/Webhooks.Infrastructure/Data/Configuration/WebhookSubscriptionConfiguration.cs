using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Webhooks.Infrastructure.Data.Configuration;

public sealed class WebhookSubscriptionConfiguration : IEntityTypeConfiguration<WebhookSubscription>
{
    public void Configure(EntityTypeBuilder<WebhookSubscription> builder)
    {
        builder.ToTable("webhooks");

        builder.Property(w => w.Id)
            .HasConversion(
                v => v.Value,
                v => new WebhookId(v));

        builder.Property(o => o.Type)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(o => o.Date);

        builder.Property(o => o.DestinationUrl)
            .HasMaxLength(2000);

        builder.Property(o => o.Token)
            .HasMaxLength(200);

        builder.Property(o => o.UserId)
            .HasConversion(
                v => v.Value,
                v => new IdentityId(v));

        builder.Property(b => b.RowVersion)
           .IsRowVersion();

        builder.HasIndex(s => s.UserId);

        builder.HasIndex(s => s.Type);

    }
}
