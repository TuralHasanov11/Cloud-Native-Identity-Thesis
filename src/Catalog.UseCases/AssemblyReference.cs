using System.Reflection;

namespace Catalog.UseCases;

public static class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}
