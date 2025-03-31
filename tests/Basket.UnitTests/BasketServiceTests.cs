using System.Security.Claims;
using System.Text;
using Basket.Api.Features.Basket;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;

namespace Basket.UnitTests;

public class BasketServiceTests
{
    private static readonly Random _random = new();
    private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public static string GenerateRandomId()
    {
        var stringBuilder = new StringBuilder(10);
        for (int i = 0; i < 10; i++)
        {
            stringBuilder.Append(_chars[_random.Next(_chars.Length)]);
        }

        return stringBuilder.ToString();
    }

    [Fact]
    public async Task GetBasketReturnsEmptyForNoUser()
    {
        var mockRepository = Substitute.For<IBasketRepository>();
        var service = new BasketService(mockRepository, NullLogger<BasketService>.Instance);
        var serverCallContext = TestServerCallContext.Create();
        serverCallContext.SetUserState("__HttpContext", new DefaultHttpContext());

        var response = await service.GetBasket(new GetBasketRequest(), serverCallContext);

        Assert.IsType<CustomerBasketResponse>(response);
        Assert.Empty(response.Items);
    }

    [Fact]
    public async Task GetBasketReturnsItemsForValidUserId()
    {
        var mockRepository = Substitute.For<IBasketRepository>();
        var basketItem = new Core.BasketAggregate.BasketItem { Id = GenerateRandomId() };
        List<Core.BasketAggregate.BasketItem> items = [basketItem];
        var customerId = GenerateRandomId();

        mockRepository.GetBasketAsync(customerId).Returns(Task.FromResult(new CustomerBasket { CustomerId = customerId, Items = items }));
        var service = new BasketService(mockRepository, NullLogger<BasketService>.Instance);
        var serverCallContext = TestServerCallContext.Create();
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity([new Claim("sub", customerId.ToString())]))
        };
        serverCallContext.SetUserState("__HttpContext", httpContext);

        var response = await service.GetBasket(new GetBasketRequest(), serverCallContext);

        Assert.IsType<CustomerBasketResponse>(response);
        Assert.Single(response.Items);
    }

    [Fact]
    public async Task GetBasketReturnsEmptyForInvalidUserId()
    {
        var mockRepository = Substitute.For<IBasketRepository>();
        var basketItem = new Core.BasketAggregate.BasketItem { Id = GenerateRandomId() };
        var customerId = GenerateRandomId();
        List<Core.BasketAggregate.BasketItem> items = [basketItem];

        mockRepository.GetBasketAsync(basketItem.Id)
            .Returns(Task.FromResult(new CustomerBasket { CustomerId = customerId, Items = items }));
        var service = new BasketService(mockRepository, NullLogger<BasketService>.Instance);
        var serverCallContext = TestServerCallContext.Create();
        var httpContext = new DefaultHttpContext();
        serverCallContext.SetUserState("__HttpContext", httpContext);

        var response = await service.GetBasket(new GetBasketRequest(), serverCallContext);

        Assert.IsType<CustomerBasketResponse>(response);
        Assert.Empty(response.Items);
    }
}
