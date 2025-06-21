namespace WebApp.Server.Features.Basket;

public sealed record UpdateBasketByUserRequest(IReadOnlyCollection<BasketQuantity> Items);
