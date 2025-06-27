namespace Ordering.Core.CustomerAggregate.Specifications;

public class CardTypeSpecification(int Id) : Specification<CardType>(x => x.Id == Id);
