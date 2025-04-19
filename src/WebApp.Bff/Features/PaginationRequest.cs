namespace WebApp.Bff.Features;

public record PaginationRequest<TCursor>(int PageSize = 10, TCursor? PageCursor = default)
    where TCursor : IEquatable<TCursor>;
