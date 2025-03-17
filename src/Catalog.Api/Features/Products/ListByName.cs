using System.ComponentModel;
using Catalog.UseCases.Products;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Catalog.Api.Features.Products;

public static class ListByName
{
    public static async Task<Ok<PaginatedItems<ProductDto, Guid>>> Handle(
        IMediator mediator,
        [AsParameters] PaginationRequest<Guid> paginationRequest,
        [Description("The name of the item to return")] string name)
    {
        return await List.Handle(mediator, paginationRequest, name, null, null);
    }
}
