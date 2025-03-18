using Ordering.Infrastructure.Idempotency;

namespace Ordering.Infrastructure.Data.Configuration;

public sealed class ClientRequestConfiguration : IEntityTypeConfiguration<ClientRequest>
{
    public void Configure(EntityTypeBuilder<ClientRequest> builder)
    {
        builder.ToTable("client_requests");

        builder.Property(cr => cr.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(cr => cr.Time)
            .IsRequired();
    }
}
