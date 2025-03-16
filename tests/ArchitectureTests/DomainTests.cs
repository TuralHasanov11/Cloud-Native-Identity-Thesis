using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace ArchitectureTests;

public class DomainTests
{
    [Fact]
    public void Entities_ShouldNotHave_PublicConstructors()
    {
        var notHavingPublicConstructorRule = new NotHavingPublicConstructorRule();

        var result = DomainModelExplorer.Entities
            .Should()
            .MeetCustomRule(notHavingPublicConstructorRule)
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void DomainEvents_ShouldBe_Immutable_and_Sealed()
    {
        var result = DomainModelExplorer.DomainEvents
            .Should()
            .BeImmutable()
            .And()
            .BeSealed()
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void DomainEvents_Should_HaveDomainEventPostfix()
    {
        var result = DomainModelExplorer.DomainEvents
            .Should()
            .HaveNameEndingWith("DomainEvent", StringComparison.OrdinalIgnoreCase)
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Entities_Should_HavePrivateParameterlessConstructor()
    {
        var entityTypes = DomainModelExplorer.Entities.GetTypes();

        var entitiesWithoutPrivateParameterlessConstructor = entityTypes
            .Where(e => e.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).Length == 0);

        Assert.Empty(entitiesWithoutPrivateParameterlessConstructor);
    }

    [Fact]
    public void Entity_WhichIsNotAggregate_ShouldNotHave_PublicMethods()
    {
        var entityTypes = DomainModelExplorer.Entities.GetTypes();

        var failingTypes = entityTypes.Where(
                e => !typeof(IAggregateRoot).IsAssignableFrom(e)
                    && e.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Length != 0).ToList();

        Assert.Empty(failingTypes);
    }
}

public class NotHavingPublicConstructorRule : ICustomRule
{
    public bool MeetsRule(TypeDefinition type)
    {
        return type.GetConstructors().All(c => !c.IsPublic);
    }
}
