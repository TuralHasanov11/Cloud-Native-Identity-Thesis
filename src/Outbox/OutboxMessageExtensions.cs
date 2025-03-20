namespace Outbox;

public static class OutboxMessageExtensions
{
    public static void UseOutbox(this ModelBuilder builder)
    {
        builder.Entity<OutboxMessage>(builder =>
        {
            builder.ToTable("outbox_messages");

            builder.HasKey(e => e.Id);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.Content)
                .IsRequired()
                .HasColumnType("jsonb");

            builder.Property(x => x.State)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.TimesSent)
                .IsRequired();

            builder.Property(x => x.CreatedOnUtc)
                .IsRequired();

            builder.Property(x => x.ProcessedOnUtc)
                .IsRequired(false);

            builder.Property(x => x.Error)
                .IsRequired(false)
                .HasColumnType("text");

            builder.Property(x => x.TransactionId)
                .IsRequired();
        });
    }
}
