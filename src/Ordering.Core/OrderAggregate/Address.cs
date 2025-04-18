namespace Ordering.Core.OrderAggregate;

public readonly record struct Address(
    [EUIIData] string Street, [EUIIData] string City, [EUIIData] string State, [EUIIData] string Country, [EUIIData] string ZipCode);
