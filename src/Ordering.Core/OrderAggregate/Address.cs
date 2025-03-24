namespace Ordering.Core.OrderAggregate;

public readonly record struct Address(string Street, string City, string State, string Country, string ZipCode);
