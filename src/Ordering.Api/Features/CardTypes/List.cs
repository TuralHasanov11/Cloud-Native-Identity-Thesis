namespace Ordering.Api.Features.CardTypes;

public static class List
{
    public static async Task<Ok<IEnumerable<CardTypeDto>>> Handle(
        ICardTypeRepository cardTypeRepository,
        CancellationToken cancellationToken)
    {
        return TypedResults.Ok(await cardTypeRepository.ListAsync(ct => ct.ToCardTypeDto(), cancellationToken));
    }
}
