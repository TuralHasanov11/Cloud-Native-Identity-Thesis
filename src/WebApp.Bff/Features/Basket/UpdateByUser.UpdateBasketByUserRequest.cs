namespace WebApp.Bff.Features.Basket;

public sealed record UpdateBasketByUserRequest(IReadOnlyCollection<BasketQuantity> Items);
