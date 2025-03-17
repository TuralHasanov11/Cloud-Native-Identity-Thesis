using System.ComponentModel;
using Catalog.UseCases.Products.DeleteById;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Catalog.Api.Features.Products;

public static class DeleteById
{
    public static async Task<Results<NoContent, NotFound>> Handle(
        IMediator mediator,
        [Description("The id of the catalog item to delete")] Guid id)
    {
        var result = await mediator.Send(new DeleteProductByIdCommand(id));

        return result.IsNotFound() ? TypedResults.NotFound() : TypedResults.NoContent();
    }
}
