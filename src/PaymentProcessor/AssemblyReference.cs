using System.Reflection;

namespace PaymentProcessor;

public static class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}
