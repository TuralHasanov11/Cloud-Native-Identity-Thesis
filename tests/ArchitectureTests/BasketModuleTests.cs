using System.Reflection;

namespace ArchitectureTests;

public class BasketModuleTests
{
    [Fact]
    public void Core_Should_Not_HaveDependencyOnOtherProjects()
    {
        var assembly = Basket.Core.AssemblyReference.Assembly;

        Assert.False(HasReference(assembly, Basket.UseCases.AssemblyReference.Assembly));
        Assert.False(HasReference(assembly, Basket.Infrastructure.AssemblyReference.Assembly));
        Assert.False(HasReference(assembly, Basket.Api.AssemblyReference.Assembly));
    }

    [Fact]
    public void UseCases_Should_Not_HaveDependencyOnOtherProjects()
    {
        var assembly = Basket.UseCases.AssemblyReference.Assembly;

        Assert.False(HasReference(assembly, Basket.Infrastructure.AssemblyReference.Assembly));
        Assert.False(HasReference(assembly, Basket.Api.AssemblyReference.Assembly));
    }

    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOnOtherProjects()
    {
        var assembly = Basket.Infrastructure.AssemblyReference.Assembly;

        Assert.False(HasReference(assembly, Basket.UseCases.AssemblyReference.Assembly));
        Assert.False(HasReference(assembly, Basket.Api.AssemblyReference.Assembly));
    }

    [Fact]
    public void Api_Should_HaveDependencyOnOtherProjects()
    {
        var assembly = Basket.Api.AssemblyReference.Assembly;

        Assert.True(HasReference(assembly, Basket.UseCases.AssemblyReference.Assembly));
        Assert.True(HasReference(assembly, Basket.Infrastructure.AssemblyReference.Assembly));
        Assert.True(HasReference(assembly, Basket.Core.AssemblyReference.Assembly));
    }

    [Fact]
    public void Handlers_Should_Have_DependencyOnCore()
    {
        var assembly = Basket.UseCases.AssemblyReference.Assembly;

        var result = Types.InAssembly(assembly)
            .That()
            .HaveNameEndingWith("QueryHandler", StringComparison.OrdinalIgnoreCase)
            .Or()
            .HaveNameEndingWith("CommandHandler", StringComparison.OrdinalIgnoreCase)
            .Should()
            .HaveDependencyOn(nameof(Basket.Core))
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    private static bool HasReference(Assembly assembly, Assembly reference)
    {
        return assembly.GetReferencedAssemblies().Any(a => a.FullName == reference.FullName);
    }
}
