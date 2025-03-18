namespace ArchitectureTests;

public class IntegrationTests
{
    [Fact]
    public void IntegrationEvents_ShouldBe_Immutable_and_Sealed()
    {
        var result = DomainModelExplorer.IntegrationEvents
            .Should()
            .BeImmutable()
            .And()
            .BeSealed()
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void IntegrationEvents_Should_HaveIntegrationEventPostfix()
    {
        var result = DomainModelExplorer.IntegrationEvents
            .Should()
            .HaveNameEndingWith("IntegrationEvent", StringComparison.OrdinalIgnoreCase)
            .GetResult();

        Assert.True(result.IsSuccessful);
    }
}
