namespace Ordering.UseCases.CardTypes.Queries;

public sealed record ListCardTypesQuery() : IQuery<IEnumerable<CardTypeDto>>;
