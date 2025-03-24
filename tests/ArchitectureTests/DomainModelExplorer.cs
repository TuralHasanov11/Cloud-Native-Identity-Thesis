using EventBus.Events;
using MassTransit;

namespace ArchitectureTests;

internal static class DomainModelExplorer
{
    public static readonly PredicateList Entities = Types.InAssemblies(
    [
        Catalog.Core.AssemblyReference.Assembly,
        Ordering.Core.AssemblyReference.Assembly,
        Basket.Core.AssemblyReference.Assembly,
        Webhooks.Core.AssemblyReference.Assembly
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
        Webhooks.Core.AssemblyReference.Assembly
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
        Webhooks.Core.AssemblyReference.Assembly
    ])
        .That()
        .AreClasses()
        .And()
        .Inherit(typeof(DomainEventBase));

    public static readonly PredicateList IntegrationEvents = Types.InAssemblies(
    [
        Catalog.Contracts.AssemblyReference.Assembly,
        Ordering.Contracts.AssemblyReference.Assembly,
        Webhooks.Contracts.AssemblyReference.Assembly,
    ])
        .That()
        .AreClasses()
        .And()
        .Inherit(typeof(IntegrationEvent));

    public static readonly PredicateList IntegrationEventHandlers = Types.InAssemblies(
    [
        Catalog.Infrastructure.AssemblyReference.Assembly,
        Ordering.Infrastructure.AssemblyReference.Assembly,
        Webhooks.Infrastructure.AssemblyReference.Assembly,
    ])
        .That()
        .AreClasses()
        .And()
        .ImplementInterface(typeof(IConsumer<>));
}
