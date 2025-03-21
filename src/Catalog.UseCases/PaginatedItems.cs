namespace Catalog.UseCases;

public class PaginatedItems<TEntity, TCursor>(
    TCursor pageCursor,
    int pageSize,
    long count,
    IEnumerable<TEntity> data)
    where TEntity : EntityBase<TCursor>
    where TCursor : class, IEquatable<TCursor>
{
    public TCursor PageCursor { get; } = pageCursor;

    public int PageSize { get; } = pageSize;

    public long Count { get; } = count;

    public IEnumerable<TEntity> Data { get; } = data;
}
