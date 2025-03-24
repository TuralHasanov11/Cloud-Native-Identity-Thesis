using System.Reflection;
using Xunit.Abstractions;

namespace ArchitectureTests;

public class DomainTests(ITestOutputHelper testOutputHelper)
{
    private readonly ITestOutputHelper _testOutputHelper = testOutputHelper;

    [Fact]
    public void Entities_ShouldNotHave_PublicConstructors()
    {
        var notHavingPublicConstructorRule = new NotHavingPublicConstructorRule();

        var result = DomainModelExplorer.Entities
            .Should()
            .MeetCustomRule(notHavingPublicConstructorRule)
            .GetResult();

        if (result.FailingTypeNames is not null)
        {
            _testOutputHelper.WriteLine($"Failing Types: {string.Join(',', result.FailingTypeNames)}");
        }

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

        if (result.FailingTypeNames is not null)
        {
            _testOutputHelper.WriteLine($"Failing Types: {string.Join(',', result.FailingTypeNames)}");
        }

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void DomainEvents_Should_HaveDomainEventPostfix()
    {
        var result = DomainModelExplorer.DomainEvents
            .Should()
            .HaveNameEndingWith("DomainEvent", StringComparison.OrdinalIgnoreCase)
            .GetResult();

        if (result.FailingTypeNames is not null)
        {
            _testOutputHelper.WriteLine($"Failing Types: {string.Join(',', result.FailingTypeNames)}");
        }

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Entities_Should_HavePrivateParameterlessConstructor()
    {
        var entityTypes = DomainModelExplorer.Entities.GetTypes();

        var entitiesWithoutPrivateParameterlessConstructor = entityTypes
            .Where(e => e.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).Length == 0);

        if (entitiesWithoutPrivateParameterlessConstructor is not null)
        {
            _testOutputHelper.WriteLine($"Failing Types: {string.Join(',', entitiesWithoutPrivateParameterlessConstructor)}");
        }

        Assert.Empty(entitiesWithoutPrivateParameterlessConstructor);
    }

    [Fact]
    public void Entity_WhichIsNotAggregate_ShouldNotHave_PublicMethods()
    {
        var entityTypes = DomainModelExplorer.Entities.GetTypes();

        var failingTypes = entityTypes.Where(
                e => !typeof(IAggregateRoot).IsAssignableFrom(e)
                    && e.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Length != 0);

        if (failingTypes is not null)
        {
            _testOutputHelper.WriteLine($"Failing Types: {string.Join(',', failingTypes)}");
        }

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
