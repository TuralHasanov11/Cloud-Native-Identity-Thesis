using Xunit.Abstractions;

namespace ArchitectureTests;

public class IntegrationTests(ITestOutputHelper testOutputHelper)
{
    private readonly ITestOutputHelper _testOutputHelper = testOutputHelper;

    [Fact]
    public void IntegrationEvents_ShouldBe_Immutable_and_Sealed()
    {
        var result = DomainModelExplorer.IntegrationEvents
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
    public void IntegrationEvents_Should_HaveIntegrationEventPostfix()
    {
        var result = DomainModelExplorer.IntegrationEvents
            .Should()
            .HaveNameEndingWith("IntegrationEvent", StringComparison.OrdinalIgnoreCase)
            .GetResult();

        if (result.FailingTypeNames is not null)
        {
            _testOutputHelper.WriteLine($"Failing Types: {string.Join(',', result.FailingTypeNames)}");
        }


        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void IntegrationEventHandlers_Should_HaveIntegrationEventHandlerPostfix()
    {
        var result = DomainModelExplorer.IntegrationEventHandlers
            .Should()
            .HaveNameEndingWith("IntegrationEventHandler", StringComparison.OrdinalIgnoreCase)
            .GetResult();

        if (result.FailingTypeNames is not null)
        {
            _testOutputHelper.WriteLine($"Failing Types: {string.Join(',', result.FailingTypeNames)}");
        }

        Assert.True(result.IsSuccessful);
    }
}
