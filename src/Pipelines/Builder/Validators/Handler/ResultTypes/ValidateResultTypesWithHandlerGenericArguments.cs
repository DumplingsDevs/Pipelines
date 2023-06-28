using Pipelines.Exceptions;
using Pipelines.Utils;

namespace Pipelines.Builder.Validators.Handler.ResultTypes;

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
        MethodResultTypesValidator.Validate(handleMethod, expectedResultTypes, handlerType);
    }

    private static Type[] GetResultTypes(Type[] genericArguments)
    {
        //24.06.2023 - Since the first generic argument is the input type (InputType), it is disregarded when obtaining the result types
        return genericArguments.Length < 1
            ? Array.Empty<Type>()
            : genericArguments.Skip(1).Take(genericArguments.Length - 1).ToArray();
    }
}
