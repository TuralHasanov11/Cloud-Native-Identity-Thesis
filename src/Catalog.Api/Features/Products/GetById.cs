using System.ComponentModel;
using Catalog.UseCases.Products;
using Catalog.UseCases.Products.List;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Catalog.Api.Features.Products;

public static class List
{
    public static async Task<Ok<PaginatedItems<ProductDto, Guid>>> Handle(
        IMediator mediator,
        [AsParameters] PaginationRequest<Guid> paginationRequest,
        [Description("The name of the item to return")] string name,
        [Description("The type of items to return")] Guid? type,
        [Description("The brand of items to return")] Guid? brand)
    {
        var pageSize = paginationRequest.PageSize;
        var pageIndex = paginationRequest.PageCursor;

        var result = await mediator.Send(new ListProductsQuery(pageIndex, pageSize, name, type, brand));

        return TypedResults.Ok(result.Value);
    }
}
