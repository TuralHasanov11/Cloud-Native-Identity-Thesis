using System.Reflection;

namespace Webhooks.UseCases;

public static class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}
