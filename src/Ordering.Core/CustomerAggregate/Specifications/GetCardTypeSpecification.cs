namespace Ordering.Core.CustomerAggregate.Specifications;

public class GetCardTypeSpecification(int Id) : Specification<CardType>(x => x.Id == Id);
