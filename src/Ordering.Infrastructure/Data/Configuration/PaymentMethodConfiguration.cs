namespace Ordering.Infrastructure.Data.Configuration;

public sealed class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable("payment_methods");

        builder.Property(b => b.Id)
            .HasConversion(
                v => v.Value,
                v => new PaymentMethodId(v));

        builder.Property(b => b.CardNumber)
            .HasMaxLength(25)
            .IsRequired();

        builder.Property(b => b.SecurityNumber)
            .HasMaxLength(10);

        builder.Property(b => b.CardHolderName)
            .HasMaxLength(200);

        builder.Property(b => b.Alias)
            .HasMaxLength(200);

        builder.Property(b => b.ExpirationDate);

        builder.HasOne(p => p.CardType)
            .WithMany()
            .HasForeignKey(p => p.CardTypeId);
    }
}
