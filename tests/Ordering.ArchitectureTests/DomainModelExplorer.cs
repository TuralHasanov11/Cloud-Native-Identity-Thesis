using NetArchTest.Rules;
using SharedKernel;

namespace Ordering.ArchitectureTests;

internal static class DomainModelExplorer
{
    public static readonly PredicateList Entities = Types.InAssembly(Core.AssemblyReference.Assembly)
        .That()
        .AreClasses()
        .And()
        .AreNotAbstract()
        .And()
    .Inherit(typeof(EntityBase));

    public static readonly PredicateList Aggregates = Types.InAssembly(Core.AssemblyReference.Assembly)
        .That()
        .AreClasses()
        .And()
        .ImplementInterface(typeof(IAggregateRoot));

    public static readonly PredicateList DomainEvents = Types.InAssembly(Core.AssemblyReference.Assembly)
        .That()
        .AreClasses()
        .And()
        .Inherit(typeof(DomainEventBase));
}
