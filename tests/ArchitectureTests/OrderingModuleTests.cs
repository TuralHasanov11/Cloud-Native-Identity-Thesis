using System.Reflection;

namespace ArchitectureTests;

public class OrderingModuleTests
{
    [Fact]
    public void Core_Should_Not_HaveDependencyOnOtherProjects()
    {
        var assembly = Ordering.Core.AssemblyReference.Assembly;

        Assert.False(HasReference(assembly, Ordering.Infrastructure.AssemblyReference.Assembly));
        Assert.False(HasReference(assembly, Ordering.Api.AssemblyReference.Assembly));
    }

    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOnOtherProjects()
    {
        var assembly = Ordering.Infrastructure.AssemblyReference.Assembly;

        Assert.False(HasReference(assembly, Ordering.Api.AssemblyReference.Assembly));
    }

    [Fact]
    public void Api_Should_HaveDependencyOnOtherProjects()
    {
        var assembly = Ordering.Api.AssemblyReference.Assembly;

        Assert.True(HasReference(assembly, Ordering.Infrastructure.AssemblyReference.Assembly));
        Assert.True(HasReference(assembly, Ordering.Core.AssemblyReference.Assembly));
    }

    private static bool HasReference(Assembly assembly, Assembly reference)
    {
        return assembly.GetReferencedAssemblies().Any(a => a.FullName == reference.FullName);
    }
}
