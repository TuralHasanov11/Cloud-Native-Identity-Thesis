using System.Reflection;

namespace ArchitectureTests;

public class WebhooksModuleTests
{
    [Fact]
    public void Core_Should_Not_HaveDependencyOnOtherProjects()
    {
        var assembly = Webhooks.Core.AssemblyReference.Assembly;

        Assert.False(HasReference(assembly, Webhooks.UseCases.AssemblyReference.Assembly));
        Assert.False(HasReference(assembly, Webhooks.Infrastructure.AssemblyReference.Assembly));
        Assert.False(HasReference(assembly, Webhooks.Api.AssemblyReference.Assembly));
    }

    [Fact]
    public void UseCases_Should_Not_HaveDependencyOnOtherProjects()
    {
        var assembly = Webhooks.UseCases.AssemblyReference.Assembly;

        Assert.False(HasReference(assembly, Webhooks.Infrastructure.AssemblyReference.Assembly));
        Assert.False(HasReference(assembly, Webhooks.Api.AssemblyReference.Assembly));
    }

    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOnOtherProjects()
    {
        var assembly = Webhooks.Infrastructure.AssemblyReference.Assembly;

        Assert.False(HasReference(assembly, Webhooks.Api.AssemblyReference.Assembly));
    }

    [Fact]
    public void Api_Should_HaveDependencyOnOtherProjects()
    {
        var assembly = Webhooks.Api.AssemblyReference.Assembly;

        Assert.True(HasReference(assembly, Webhooks.UseCases.AssemblyReference.Assembly));
        Assert.True(HasReference(assembly, Webhooks.Infrastructure.AssemblyReference.Assembly));
        Assert.True(HasReference(assembly, Webhooks.Core.AssemblyReference.Assembly));
    }

    private static bool HasReference(Assembly assembly, Assembly reference)
    {
        return assembly.GetReferencedAssemblies().Any(a => a.FullName == reference.FullName);
    }
}
