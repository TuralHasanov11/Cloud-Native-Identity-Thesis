using Ordering.UseCases.CardTypes;
using Ordering.UseCases.CardTypes.Queries;

namespace Ordering.Api.Features.CardTypes;

public static class List
{
    public static async Task<Ok<IEnumerable<CardTypeDto>>> Handle(
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ListCardTypesQuery(), cancellationToken);

        return TypedResults.Ok(result.Value);
    }
}
