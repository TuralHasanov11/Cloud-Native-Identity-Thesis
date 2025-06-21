using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SharedKernel;

public abstract class EntityBase : HasDomainEventsBase
{
    protected EntityBase()
    {
    }

    public int Id { get; }

    public byte[]? RowVersion { get; }

    //public DateTime CreatedOnUtc { get; set; }

    //public DateTime UpdatedOnUtc { get; set; }
}

public abstract class EntityBase<TId> : HasDomainEventsBase
  where TId : notnull, IEquatable<TId>
{
    protected EntityBase()
    {
    }

    protected EntityBase(TId id)
    {
        Id = id;
    }

    public TId Id { get; }

    public byte[]? RowVersion { get; }

    public string RowVersionValue => RowVersion is not null ? Convert.ToBase64String(RowVersion) : string.Empty;

    //public DateTime CreatedOnUtc { get; set; }

    //public DateTime UpdatedOnUtc { get; set; }
}


public static class EntityBaseExtensions
{
    //public static void ConfigureTimestamps(this EntityTypeBuilder<EntityBase> builder)
    //{
    //    builder.Property(b => b.CreatedOnUtc)
    //        .HasDefaultValueSql("CURRENT_TIMESTAMP")
    //        .ValueGeneratedOnAdd();

    //    builder.Property(b => b.UpdatedOnUtc)
    //        .HasDefaultValueSql("CURRENT_TIMESTAMP")
    //        .ValueGeneratedOnAddOrUpdate();
    //}
}
