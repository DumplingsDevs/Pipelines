using System.Reflection;
using Pipelines.Exceptions;

namespace Pipelines.Utils;

internal static class MethodResultTypesValidator
{
    internal static void Validate(MethodInfo methodToValidate, Type[] expectedResultTypes, Type handlerType)
    {
        var isVoidMethod = methodToValidate.IsVoidOrTaskReturnType();
        var methodReturnTypes = methodToValidate.GetReturnTypes();

        if (isVoidMethod)
        {
            ValidateVoidMethod(isVoidMethod, expectedResultTypes, handlerType);
        }
        else
        {
            ValidateResultTypeCount(expectedResultTypes.Length, methodReturnTypes.Count);
            ValidateResultTypeMatch(expectedResultTypes, methodReturnTypes);
        }
    }
    
    private static void ValidateResultTypeMatch(Type[] expectedResultTypes, List<Type> methodReturnTypes)
    {
        for (var i = 0; i < expectedResultTypes.Length; i++)
        {
            var expectedResultType = expectedResultTypes[i];
            var methodReturnType = methodReturnTypes[i];

            if (expectedResultType != methodReturnType)
            {
                throw new ReturnTypeMismatchException(expectedResultType, methodReturnType);
            }
        }
    }

    private static void ValidateVoidMethod(bool isVoidMethod, Type[] expectedResultTypes, Type handlerType)
    {
        switch (isVoidMethod)
        {
            case true when expectedResultTypes.Any():
                throw new ExpectedMethodWithResultException(handlerType);

            case false when !expectedResultTypes.Any():
                throw new ExpectedVoidMethodException(handlerType);
        }
    }

    private static void ValidateResultTypeCount(int expectedResultTypesLength, int methodReturnTypesLength)
    {
        if (expectedResultTypesLength != methodReturnTypesLength)
        {
            throw new ResultTypeCountMismatchException(expectedResultTypesLength, methodReturnTypesLength);
        }
    }
}