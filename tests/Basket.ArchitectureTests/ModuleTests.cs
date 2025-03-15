using System.Reflection;
using NetArchTest.Rules;

namespace Basket.ArchitectureTests;

public class ModuleTests
{
    [Fact]
    public void Core_Should_Not_HaveDependencyOnOtherProjects()
    {
        var assembly = Core.AssemblyReference.Assembly;

        Assert.False(HasReference(assembly, UseCases.AssemblyReference.Assembly));
        Assert.False(HasReference(assembly, Infrastructure.AssemblyReference.Assembly));
        Assert.False(HasReference(assembly, Api.AssemblyReference.Assembly));
    }

    [Fact]
    public void UseCases_Should_Not_HaveDependencyOnOtherProjects()
    {
        var assembly = UseCases.AssemblyReference.Assembly;

        Assert.False(HasReference(assembly, Infrastructure.AssemblyReference.Assembly));
        Assert.False(HasReference(assembly, Api.AssemblyReference.Assembly));
    }

    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOnOtherProjects()
    {
        var assembly = Infrastructure.AssemblyReference.Assembly;

        Assert.False(HasReference(assembly, UseCases.AssemblyReference.Assembly));
        Assert.False(HasReference(assembly, Api.AssemblyReference.Assembly));
    }

    [Fact]
    public void Api_Should_HaveDependencyOnOtherProjects()
    {
        var assembly = Api.AssemblyReference.Assembly;

        Assert.True(HasReference(assembly, UseCases.AssemblyReference.Assembly));
        Assert.True(HasReference(assembly, Infrastructure.AssemblyReference.Assembly));
        Assert.True(HasReference(assembly, Core.AssemblyReference.Assembly));
    }

    [Fact]
    public void Handlers_Should_Have_DependencyOnCore()
    {
        var assembly = UseCases.AssemblyReference.Assembly;

        var result = Types.InAssembly(assembly)
            .That()
            .HaveNameEndingWith("QueryHandler", StringComparison.OrdinalIgnoreCase)
            .Or()
            .HaveNameEndingWith("CommandHandler", StringComparison.OrdinalIgnoreCase)
            .Should()
            .HaveDependencyOn(nameof(Core))
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    private static bool HasReference(Assembly assembly, Assembly reference)
    {
        return assembly.GetReferencedAssemblies().Any(a => a.FullName == reference.FullName);
    }
}
