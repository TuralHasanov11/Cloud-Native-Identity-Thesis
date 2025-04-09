namespace Ordering.Infrastructure.Data.Configuration;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_items");

        builder.Property(o => o.Id)
            .HasConversion(
                v => v.Value,
                v => new OrderItemId(v));

        builder.Property(o => o.ProductName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(o => o.UnitPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(o => o.Discount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(o => o.PictureUrl)
            .HasMaxLength(2000);

        builder.Property(o => o.Units)
            .IsRequired();

        builder.Property(o => o.ProductId)
            .IsRequired();

        builder.HasOne<Order>()
            .WithMany(o => o.OrderItems)
            .HasForeignKey(o => o.OrderId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
