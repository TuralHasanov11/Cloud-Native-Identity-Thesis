namespace Catalog.Core.CatalogAggregate;

public sealed record ProductId(Guid Value) : IComparable<ProductId>
{
    public int CompareTo(ProductId? other)
    {
        return other is null ? 1 : Value.CompareTo(other.Value);
    }

    public static implicit operator Guid(ProductId self) => self.Value;

    public static bool operator >(ProductId operand1, ProductId operand2)
    {
        return operand1.CompareTo(operand2) > 0;
    }

    public static bool operator <(ProductId operand1, ProductId operand2)
    {
        return operand1.CompareTo(operand2) < 0;
    }

    public static bool operator >=(ProductId operand1, ProductId operand2)
    {
        return operand1.CompareTo(operand2) >= 0;
    }

    public static bool operator <=(ProductId operand1, ProductId operand2)
    {
        return operand1.CompareTo(operand2) <= 0;
    }
}
