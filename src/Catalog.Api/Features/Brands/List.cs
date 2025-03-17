using Catalog.UseCases.Brands;
using Catalog.UseCases.Brands.List;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Catalog.Api.Features.Brands;

public static class List
{
    public static async Task<Ok<IEnumerable<BrandDto>>> Handle(
        IMediator mediator)
    {
        var result = await mediator.Send(new ListBrandsQuery());

        return TypedResults.Ok(result.Value);
    }
}
