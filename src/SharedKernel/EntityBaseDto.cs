namespace SharedKernel;

public abstract record EntityBaseDto(int Id, DateTime CreatedOnUtc, DateTime? UpdateOnUtc);

public abstract record EntityBaseDto<TId>(TId Id, DateTime CreatedOnUtc, DateTime? UpdateOnUtc)
    where TId : notnull, IEquatable<TId>;