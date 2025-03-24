using System.Reflection;

namespace ArchitectureTests;

public class CatalogModuleTests
{
    [Fact]
    public void Core_Should_Not_HaveDependencyOnOtherProjects()
    {
        var assembly = Catalog.Core.AssemblyReference.Assembly;

        Assert.False(HasReference(assembly, Catalog.UseCases.AssemblyReference.Assembly));
        Assert.False(HasReference(assembly, Catalog.Infrastructure.AssemblyReference.Assembly));
        Assert.False(HasReference(assembly, Catalog.Api.AssemblyReference.Assembly));
    }

    [Fact]
    public void UseCases_Should_Not_HaveDependencyOnOtherProjects()
    {
        var assembly = Catalog.UseCases.AssemblyReference.Assembly;

        Assert.False(HasReference(assembly, Catalog.Infrastructure.AssemblyReference.Assembly));
        Assert.False(HasReference(assembly, Catalog.Api.AssemblyReference.Assembly));
    }

    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOnOtherProjects()
    {
        var assembly = Catalog.Infrastructure.AssemblyReference.Assembly;

        Assert.False(HasReference(assembly, Catalog.Api.AssemblyReference.Assembly));
    }

    [Fact]
    public void Api_Should_HaveDependencyOnOtherProjects()
    {
        var assembly = Catalog.Api.AssemblyReference.Assembly;

        Assert.True(HasReference(assembly, Catalog.UseCases.AssemblyReference.Assembly));
        Assert.True(HasReference(assembly, Catalog.Infrastructure.AssemblyReference.Assembly));
        Assert.True(HasReference(assembly, Catalog.Core.AssemblyReference.Assembly));
    }

    private static bool HasReference(Assembly assembly, Assembly reference)
    {
        return assembly.GetReferencedAssemblies().Any(a => a.FullName == reference.FullName);
    }
}
