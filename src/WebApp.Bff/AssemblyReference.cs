using System.Reflection;

namespace WebApp.Bff;

public static class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}
