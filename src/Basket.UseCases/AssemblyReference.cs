using System.Reflection;

namespace Basket.UseCases;

public static class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}
