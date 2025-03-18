using EventBus.Events;

namespace ArchitectureTests;

internal static class DomainModelExplorer
{
    public static readonly PredicateList Entities = Types.InAssemblies(
    [
        Catalog.Core.AssemblyReference.Assembly,
        Ordering.Core.AssemblyReference.Assembly,
        Basket.Core.AssemblyReference.Assembly,
    ])
        .That()
        .AreClasses()
        .And()
        .AreNotAbstract()
        .And()
    .Inherit(typeof(EntityBase));

    public static readonly PredicateList Aggregates = Types.InAssemblies(
    [
        Catalog.Core.AssemblyReference.Assembly,
        Ordering.Core.AssemblyReference.Assembly,
        Basket.Core.AssemblyReference.Assembly,
    ])
        .That()
        .AreClasses()
        .And()
        .ImplementInterface(typeof(IAggregateRoot));

    public static readonly PredicateList DomainEvents = Types.InAssemblies(
    [
        Catalog.Core.AssemblyReference.Assembly,
        Ordering.Core.AssemblyReference.Assembly,
        Basket.Core.AssemblyReference.Assembly,
    ])
        .That()
        .AreClasses()
        .And()
        .Inherit(typeof(DomainEventBase));

    public static readonly PredicateList IntegrationEvents = Types.InAssemblies(
    [
        Catalog.Contracts.AssemblyReference.Assembly,
        Ordering.Contracts.AssemblyReference.Assembly,
    ])
        .That()
        .AreClasses()
        .And()
        .Inherit(typeof(IntegrationEvent));
}
