using Xunit.Abstractions;

namespace ArchitectureTests;

public class UseCaseTests(ITestOutputHelper testOutputHelper)
{
    private readonly ITestOutputHelper _testOutputHelper = testOutputHelper;

    private static readonly PredicateList QueryHandlers = Types.InAssemblies([
            Basket.UseCases.AssemblyReference.Assembly,
            Catalog.UseCases.AssemblyReference.Assembly,
            Ordering.UseCases.AssemblyReference.Assembly,
            Webhooks.UseCases.AssemblyReference.Assembly
        ])
        .That()
        .ImplementInterface(typeof(IQueryHandler<,>));

    private static readonly PredicateList Queries = Types.InAssemblies([
            Basket.UseCases.AssemblyReference.Assembly,
            Catalog.UseCases.AssemblyReference.Assembly,
            Ordering.UseCases.AssemblyReference.Assembly,
            Webhooks.UseCases.AssemblyReference.Assembly
        ])
        .That()
        .ImplementInterface(typeof(IQuery<>));

    private static readonly PredicateList Commands = Types.InAssemblies([
            Basket.UseCases.AssemblyReference.Assembly,
            Catalog.UseCases.AssemblyReference.Assembly,
            Ordering.UseCases.AssemblyReference.Assembly,
            Webhooks.UseCases.AssemblyReference.Assembly
        ])
        .That()
        .ImplementInterface(typeof(ICommand))
        .Or()
        .ImplementInterface(typeof(ICommand<>));

    private static readonly PredicateList CommandHandlers = Types.InAssemblies([
            Basket.UseCases.AssemblyReference.Assembly,
            Catalog.UseCases.AssemblyReference.Assembly,
            Ordering.UseCases.AssemblyReference.Assembly,
            Webhooks.UseCases.AssemblyReference.Assembly
        ])
        .That()
        .ImplementInterface(typeof(ICommandHandler<>))
        .Or()
        .ImplementInterface(typeof(ICommandHandler<,>));


    [Fact]
    public void Query_ShouldHave_NameWithPostfix_Query()
    {
        var result = Queries
            .Should()
            .HaveNameEndingWith("Query", StringComparison.OrdinalIgnoreCase)
            .GetResult();

        if (result.FailingTypeNames is not null)
        {
            _testOutputHelper.WriteLine($"Failing Types: {string.Join(',', result.FailingTypeNames)}");
        }

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Command_ShouldHave_NameWithPostfix_Command()
    {
        var result = Commands
            .Should()
            .HaveNameEndingWith("Command", StringComparison.OrdinalIgnoreCase)
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void QueryHandler_ShouldHave_NameWithPostfix_Handler()
    {
        var result = QueryHandlers
            .Should()
            .HaveNameEndingWith("QueryHandler", StringComparison.OrdinalIgnoreCase)
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void CommandHandler_ShouldHave_NameWithPostfix_Handler()
    {
        var result = CommandHandlers
            .Should()
            .HaveNameEndingWith("CommandHandler", StringComparison.OrdinalIgnoreCase)
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Query_ShouldBe_SealedAndImmutable()
    {
        var result = Queries
            .Should()
            .BeSealed()
            .And()
            .MeetCustomRule(new BeImmutableIncludingRecordsRule())
            .GetResult();

        if (result.FailingTypeNames is not null)
        {
            _testOutputHelper.WriteLine($"Failing Types: {string.Join(',', result.FailingTypeNames)}");
        }

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Command_ShouldBe_SealedAndImmutable()
    {
        var result = Commands
            .Should()
            .BeSealed()
            .And()
            .MeetCustomRule(new BeImmutableIncludingRecordsRule())
            .GetResult();

        if (result.FailingTypeNames is not null)
        {
            _testOutputHelper.WriteLine($"Failing Commands: {string.Join(',', result.FailingTypeNames)}");
        }

        Assert.True(result.IsSuccessful);
    }
}

public class BeImmutableIncludingRecordsRule : ICustomRule
{
    public bool MeetsRule(TypeDefinition type)
    {
        return type.GetMethods().Any(m => m.Name switch
        {
            "PrintMembers" => true,
            "Equals" => true,
            "GetHashCode" => true,
            _ => false
        });
    }
}
