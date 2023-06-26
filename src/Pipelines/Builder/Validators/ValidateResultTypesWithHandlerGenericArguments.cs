using Pipelines.Exceptions;
using Pipelines.Utils;

namespace Pipelines.Builder.Validators;

internal static class ValidateResultTypesWithHandlerGenericArguments
{
    internal static void Validate(Type handlerType)
    {
        ParamValidator.NotNull(handlerType, nameof(handlerType));
        
        var genericArguments = handlerType.GetGenericArguments();
        var handleMethod = handlerType.GetMethods().FirstOrDefault();

        if (handleMethod == null)
        {
            throw new HandleMethodNotFoundException(handlerType);
        }

        var expectedResultTypes = GetResultTypes(genericArguments);
        var isVoidMethod = handleMethod.IsVoidOrTaskReturnType();
        var methodReturnTypes = handleMethod.GetReturnTypes();

        ValidateVoidMethod(isVoidMethod, expectedResultTypes, handlerType);
        
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
            var expectedResultType = expectedResultTypes[i];
            var methodReturnType = methodReturnTypes[i];

            if (expectedResultType != methodReturnType)
            {
                throw new ReturnTypeMismatchException(expectedResultType, methodReturnType);
            }
        }
    }

    private static void ValidateVoidMethod(bool isVoidMethod, List<Type> expectedResultTypes, Type handlerType)
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

    private static List<Type> GetResultTypes(Type[] genericArguments)
    {
        //24.06.2023 - Since the first generic argument is the input type (InputType), it is disregarded when obtaining the result types
        return genericArguments.Length < 1
            ? new List<Type>()
            : genericArguments.Skip(1).Take(genericArguments.Length - 1).ToList();
    }
}
