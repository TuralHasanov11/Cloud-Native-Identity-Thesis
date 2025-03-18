namespace Ordering.Infrastructure.Data.Configuration;

public sealed class CardTypeConfiguration : IEntityTypeConfiguration<CardType>
{
    public void Configure(EntityTypeBuilder<CardType> builder)
    {
        builder.ToTable("card_types");

        builder.Property(ct => ct.Id)
            .ValueGeneratedNever();

        builder.Property(ct => ct.Name)
            .HasMaxLength(200)
            .IsRequired();
    }
}
