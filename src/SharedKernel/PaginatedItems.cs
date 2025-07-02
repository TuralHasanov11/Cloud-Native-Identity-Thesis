namespace SharedKernel;

public class PaginatedItems<TEntity, TCursor>(
    TCursor pageCursor,
    int pageSize,
    long count,
    IEnumerable<TEntity> data)
    where TEntity : class
    where TCursor : IEquatable<TCursor>, IComparable<TCursor>
{
    public TCursor? PageCursor { get; } = pageCursor;

    public int PageSize { get; } = pageSize;

    public long Count { get; } = count;

    public IEnumerable<TEntity> Data { get; } = data;
}
