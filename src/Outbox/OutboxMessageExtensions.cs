namespace Outbox;

public static class OutboxMessageExtensions
{
    public static void UseOutbox(this ModelBuilder builder)
    {
        builder.Entity<OutboxMessage>(b =>
        {
            b.ToTable("outbox_messages");

            b.HasKey(e => e.Id);

            b.Property(x => x.Type)
                .IsRequired()
                .HasMaxLength(255);

            b.Property(x => x.Content)
                .IsRequired()
                .HasColumnType("jsonb");

            b.Property(x => x.State)
                .IsRequired()
                .HasConversion<string>();

            b.Property(x => x.TimesSent)
                .IsRequired();

            b.Property(x => x.CreatedOnUtc)
                .IsRequired();

            b.Property(x => x.ProcessedOnUtc)
                .IsRequired(false);

            b.Property(x => x.Error)
                .IsRequired(false)
                .HasColumnType("text");

            b.Property(x => x.TransactionId)
                .IsRequired();

            b.Ignore(x => x.IntegrationEvent);
        });
    }
}
