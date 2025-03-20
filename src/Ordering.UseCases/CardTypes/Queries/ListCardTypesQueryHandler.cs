namespace Ordering.UseCases.CardTypes.Queries;

public sealed class ListCardTypesQueryHandler(ICardTypeRepository cardTypeRepository)
    : IQueryHandler<ListCardTypesQuery, IEnumerable<CardTypeDto>>
{
    public async Task<Result<IEnumerable<CardTypeDto>>> Handle(
        ListCardTypesQuery request,
        CancellationToken cancellationToken)
    {
        return Result.Success(await cardTypeRepository.ListAsync(ct => ct.ToCardTypeDto(), cancellationToken));
    }
}
