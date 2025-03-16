using System.Reflection;

namespace ArchitectureTests;

public class OrderingModuleTests
{
    [Fact]
    public void Core_Should_Not_HaveDependencyOnOtherProjects()
    {
        var assembly = Ordering.Core.AssemblyReference.Assembly;

        Assert.False(HasReference(assembly, Ordering.UseCases.AssemblyReference.Assembly));
        Assert.False(HasReference(assembly, Ordering.Infrastructure.AssemblyReference.Assembly));
        Assert.False(HasReference(assembly, Ordering.Api.AssemblyReference.Assembly));
    }

    [Fact]
    public void UseCases_Should_Not_HaveDependencyOnOtherProjects()
    {
        var assembly = Ordering.UseCases.AssemblyReference.Assembly;

        Assert.False(HasReference(assembly, Ordering.Infrastructure.AssemblyReference.Assembly));
        Assert.False(HasReference(assembly, Ordering.Api.AssemblyReference.Assembly));
    }

    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOnOtherProjects()
    {
        var assembly = Ordering.Infrastructure.AssemblyReference.Assembly;

        Assert.False(HasReference(assembly, Ordering.UseCases.AssemblyReference.Assembly));
        Assert.False(HasReference(assembly, Ordering.Api.AssemblyReference.Assembly));
    }

    [Fact]
    public void Api_Should_HaveDependencyOnOtherProjects()
    {
        var assembly = Ordering.Api.AssemblyReference.Assembly;

        Assert.True(HasReference(assembly, Ordering.UseCases.AssemblyReference.Assembly));
        Assert.True(HasReference(assembly, Ordering.Infrastructure.AssemblyReference.Assembly));
        Assert.True(HasReference(assembly, Ordering.Core.AssemblyReference.Assembly));
    }

    [Fact]
    public void Handlers_Should_Have_DependencyOnCore()
    {
        var assembly = Ordering.UseCases.AssemblyReference.Assembly;

        var result = Types.InAssembly(assembly)
            .That()
            .HaveNameEndingWith("QueryHandler", StringComparison.OrdinalIgnoreCase)
            .Or()
            .HaveNameEndingWith("CommandHandler", StringComparison.OrdinalIgnoreCase)
            .Should()
            .HaveDependencyOn(nameof(Ordering.Core))
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    private static bool HasReference(Assembly assembly, Assembly reference)
    {
        return assembly.GetReferencedAssemblies().Any(a => a.FullName == reference.FullName);
    }
}
