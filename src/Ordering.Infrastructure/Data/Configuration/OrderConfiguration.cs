﻿namespace Ordering.Infrastructure.Data.Configuration;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.Property(o => o.Id)
            .HasConversion(
                v => v.Value,
                v => new OrderId(v));

        builder.ComplexProperty(
            o => o.Address,
            o =>
            {
                o.Property(a => a.Street)
                    .HasMaxLength(180)
                    .IsRequired();

                o.Property(a => a.City)
                    .HasMaxLength(100)
                    .IsRequired();

                o.Property(a => a.State)
                    .HasMaxLength(60);

                o.Property(a => a.Country)
                    .HasMaxLength(90)
                    .IsRequired();

                o.Property(a => a.ZipCode)
                    .HasMaxLength(18)
                    .IsRequired();
            });

        builder.Property(o => o.OrderDate);

        builder.Property(o => o.Description)
            .HasMaxLength(1000);

        builder.Property(o => o.OrderStatus)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(o => o.IsDraft);

        builder.Property(b => b.RowVersion)
            .IsRowVersion();

        builder.HasOne<PaymentMethod>()
            .WithMany()
            .HasForeignKey(o => o.PaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Customer)
            .WithMany()
            .HasForeignKey(o => o.CustomerId);
    }
}
