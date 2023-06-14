using System.Reflection;
using Pipelines.Exceptions;

namespace Pipelines.Builder.Validators;

internal static class ShouldImplementExactlySameHandleMethods
{
    internal static void Validate(params Type[] typesToValidate)
    {
        if (typesToValidate.Length < 2)
        {
            throw new ArgumentException("At least two types must be provided for comparison.");
        }

        var methodsSet = GetHandleMethod(typesToValidate[0]);

        for (var i = 1; i < typesToValidate.Length; i++)
        {
            var currentType = typesToValidate[i];
            var currentMethodsSet = GetHandleMethod(currentType);

            if (!AreMethodsEqual(methodsSet, currentMethodsSet))
            {
                throw new HandleMethodMismatchException(typesToValidate[0], typesToValidate[i]);
            }
        }
    }

    private static string GetHandleMethod(Type type)
    {
        var method = type.GetMethods()
            .Select(m => $"{GetParametersSignature(m)}-{m.ReturnType.FullName}").First();
        
        return method;
    }

    private static string GetParametersSignature(MethodInfo method)
    {
        var parameterTypes = method.GetParameters()
            .Select(p => p.ParameterType.FullName);
        return string.Join(",", parameterTypes);
    }

    private static bool AreMethodsEqual(string method1, string method2)
    {
        return method1.Equals(method2, StringComparison.Ordinal);
    }
}