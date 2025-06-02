using System.Diagnostics;
using Basket.Api.Grpc;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Web;
using GrpcBasketClient = Basket.Api.Grpc.Basket.BasketClient;
using GrpcBasketItem = Basket.Api.Grpc.BasketItem;

namespace WebApp.Bff.Features.Basket;

public class BasketService
{
    private readonly GrpcBasketClient _basketClient;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly HttpContext? _httpContext;

    public BasketService(
        GrpcBasketClient basketClient, 
        IServiceProvider serviceProvider, 
        IConfiguration configuration, 
        IHttpContextAccessor httpContextAccessor)
    {
        _basketClient = basketClient;
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _httpContext = httpContextAccessor.HttpContext;
    }

    public async Task<IReadOnlyCollection<BasketQuantity>> GetBasketAsync()
    {
        string accessToken = await GetAccessToken();

        var headers = new Metadata
        {
            { "Authorization", $"Bearer {accessToken}" }
        };

        var result = await _basketClient.GetBasketAsync(new(), headers);
        return MapToBasket(result);
    }

    public async Task DeleteBasketAsync()
    {
        string accessToken = await GetAccessToken();

        var headers = new Metadata
        {
            { "Authorization", $"Bearer {accessToken}" }
        };

        await _basketClient.DeleteBasketAsync(new DeleteBasketRequest(), headers);
    }

    public async Task<IEnumerable<BasketQuantity>> UpdateBasketAsync(IReadOnlyCollection<BasketQuantity> basket)
    {
        string accessToken = await GetAccessToken();

        var headers = new Metadata
        {
            { "Authorization", $"Bearer {accessToken}" }
        };

        var updatePayload = new UpdateBasketRequest();

        foreach (var item in basket)
        {
            var updateItem = new GrpcBasketItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
            };
            updatePayload.Items.Add(updateItem);
        }

        var response = await _basketClient.UpdateBasketAsync(updatePayload, headers);

        return response.Items.Select(x => new BasketQuantity(x.ProductId, x.Quantity));
    }

    private static List<BasketQuantity> MapToBasket(CustomerBasketResponse response)
    {
        var result = new List<BasketQuantity>();
        foreach (var item in response.Items)
        {
            result.Add(new BasketQuantity(item.ProductId, item.Quantity));
        }

        return result;
    }

    private async Task<string> GetAccessToken()
    {
        ITokenAcquisition? tokenAcquisition = _serviceProvider.GetService<ITokenAcquisition>();

        // Azure
        if (tokenAcquisition is not null)
        {
            var scopes = _configuration[$"{IdentityProviderSettings.AzureAd}:Scopes"]?.Split(" ", StringSplitOptions.RemoveEmptyEntries) ?? [];

            try
            {
                return await tokenAcquisition.GetAccessTokenForUserAsync(scopes);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"No access token: {ex.Message}");
                return string.Empty;
            }
        }
        else
        {
            try
            {
                return await _httpContext.GetTokenAsync("access_token") ?? string.Empty;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"No access token: {ex.Message}");
                return string.Empty;
            }
        }
    }
}

public record BasketQuantity(string ProductId, int Quantity);
