namespace Ordering.Infrastructure.Data.Configuration;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.Property(o => o.Id)
            .HasConversion(
                v => v.Value,
                v => new OrderId(v));

        builder.ComplexProperty(o => o.Address);

        builder.Property(o => o.OrderDate);

        builder.Property(o => o.Description)
            .HasMaxLength(1000);

        builder.Property(o => o.OrderStatus)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(o => o.IsDraft);

        builder.HasOne<PaymentMethod>()
            .WithMany()
            .HasForeignKey(o => o.PaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Customer)
            .WithMany()
            .HasForeignKey(o => o.CustomerId);
    }
}
