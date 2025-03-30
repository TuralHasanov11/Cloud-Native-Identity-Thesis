using System.Security.Claims;
using Basket.Api.Features.Basket;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;

namespace Basket.UnitTests;

public class BasketServiceTests
{
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
        var basketItem = new Core.BasketAggregate.BasketItem { Id = Guid.CreateVersion7() };
        List<Core.BasketAggregate.BasketItem> items = [basketItem];
        var customerId = Guid.CreateVersion7();

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
        var basketItem = new Core.BasketAggregate.BasketItem { Id = Guid.CreateVersion7() };
        var customerId = Guid.CreateVersion7();
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
