namespace Ordering.Infrastructure.Data.Configuration;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers");

        builder.Property(b => b.Id)
            .HasConversion(
                v => v.Value,
                v => new CustomerId(v));

        builder.Property(b => b.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(b => b.IdentityId)
            .HasConversion(
                v => v.Value,
                v => new IdentityId(v))
            .HasMaxLength(255);

        builder.Property(b => b.RowVersion)
            .IsRowVersion();

        builder.HasIndex(b => b.IdentityId)
            .IsUnique(true);

        builder.HasMany(b => b.PaymentMethods)
            .WithOne();
    }
}
