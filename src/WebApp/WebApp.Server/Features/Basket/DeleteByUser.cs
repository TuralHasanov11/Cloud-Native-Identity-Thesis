namespace WebApp.Server.Features.Basket;

public static class DeleteByUser
{
    public static async Task<IResult> Handle(
        BasketService basketService,
        CancellationToken _)
    {
        try
        {
            await basketService.DeleteBasketAsync();

            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}
