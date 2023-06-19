using Pipelines.Utils;

namespace Pipelines.Builder.Validators;

internal static class ValidateHandlerHandleMethod
{
    internal static void Validate(Type handlerType)
    {
        var genericArguments = handlerType.GetGenericArguments();
        var handleMethod = handlerType.GetMethods().FirstOrDefault();

        if (handleMethod == null)
        {
            throw new Exception();
        }

        var expectedResultTypes = GetResultTypes(genericArguments);
        var isVoidMethod = handleMethod.IsVoidOrTaskReturnType();
        var methodReturnTypes = handleMethod.GetReturnTypes();

        ValidateVoidMethod(isVoidMethod, expectedResultTypes);
        
        if (!isVoidMethod)
        {
            ValidateResultTypeCount(expectedResultTypes.Count, methodReturnTypes.Count);
        }
        
        ValidateResultTypeMatch(expectedResultTypes, methodReturnTypes);
    }

    private static void ValidateResultTypeMatch(List<Type> expectedResultTypes, List<Type> methodReturnTypes)
    {
        for (var i = 0; i < expectedResultTypes.Count; i++)
        {
            if (expectedResultTypes[0].FullName != methodReturnTypes[0].FullName)
            {
                throw new Exception("Return type mismatch");
            }
        }
    }

    private static void ValidateVoidMethod(bool isVoidMethod, List<Type> expectedResultTypes)
    {
        switch (isVoidMethod)
        {
            case true when expectedResultTypes.Any():
                throw new Exception("Expected method with result");

            case false when !expectedResultTypes.Any():
                throw new Exception("Expected void");
        }
    }

    private static void ValidateResultTypeCount(int expectedResultTypesLength,
        int methodReturnTypesLength)
    {
        if (expectedResultTypesLength != methodReturnTypesLength)
        {
            throw new Exception("Result type count mismatch");
        }
    }

    private static List<Type> GetResultTypes(Type[] genericArguments)
    {
        return genericArguments.Length < 1
            ? new List<Type>()
            : genericArguments.Skip(1).Take(genericArguments.Length - 1).ToList();
    }
}