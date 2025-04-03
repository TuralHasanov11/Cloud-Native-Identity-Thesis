using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Data.Configuration;

public sealed class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable("brands");

        builder.Property(b => b.Id)
            .HasConversion(
                id => id.Value,
                value => new BrandId(value));

        builder.Property(b => b.Name)
            .HasMaxLength(100);

        builder.Property(b => b.RowVersion)
            .IsRowVersion();

        builder.HasIndex(b => b.Name)
            .IsUnique();
    }
}
