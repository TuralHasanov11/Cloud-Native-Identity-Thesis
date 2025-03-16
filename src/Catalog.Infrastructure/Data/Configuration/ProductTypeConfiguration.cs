using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Data.Configuration;

public sealed class ProductTypeConfiguration : IEntityTypeConfiguration<ProductType>
{
    public void Configure(EntityTypeBuilder<ProductType> builder)
    {
        builder.ToTable("ProductTypes");

        builder.Property(pt => pt.Id)
            .HasConversion(
                id => id.Value,
                value => new ProductTypeId(value));

        builder.Property(pt => pt.Name)
            .HasMaxLength(100);

        builder.Property(pt => pt.RowVersion)
            .IsRowVersion();

        builder.HasIndex(pt => pt.Name)
            .IsUnique();
    }
}
