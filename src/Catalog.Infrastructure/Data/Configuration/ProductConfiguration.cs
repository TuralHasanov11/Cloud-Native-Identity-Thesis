using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Data.Configuration;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => new ProductId(value));

        builder.Property(p => p.Name)
            .HasMaxLength(50);

        builder.Property(p => p.Description)
            .HasColumnType("text");

        //builder.Property(p => p.Embedding)
        //    .HasColumnType("vector(384)");

        builder.HasOne(p => p.Brand)
            .WithMany();

        builder.Property(p => p.BrandId)
            .HasConversion(
                id => id.Value,
                value => new BrandId(value));

        builder.HasOne(p => p.ProductType)
            .WithMany();

        builder.Property(p => p.ProductTypeId)
            .HasConversion(
                id => id.Value,
                value => new ProductTypeId(value));

        builder.Property(p => p.Price);

        builder.Property(p => p.PictureFileName)
            .IsRequired(false)
            .HasMaxLength(200);

        builder.Property(p => p.AvailableStock);

        builder.Property(p => p.RestockThreshold);

        builder.Property(p => p.MaxStockThreshold);

        builder.Property(p => p.OnReorder);

        builder.Property(p => p.RowVersion)
            .IsRowVersion();

        builder.HasIndex(p => p.Name);
    }
}
