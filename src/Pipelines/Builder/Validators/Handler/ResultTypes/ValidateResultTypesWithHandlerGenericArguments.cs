using Pipelines.Builder.Validators.Shared.MethodResultTypes;
using Pipelines.Utils;

namespace Pipelines.Builder.Validators.Handler.ResultTypes;

internal static class ValidateResultTypesWithHandlerGenericArguments
{
    internal static void Validate(Type handlerType)
    {
        ParamValidator.NotNull(handlerType, nameof(handlerType));

        // generic arguments defined in Handler type for e.g IHandler<TInput,TResult>
        var genericArguments = handlerType.GetGenericArguments();
        var handleMethod = handlerType.GetMethods().First();

        var expectedResultTypes = GetResultTypes(genericArguments);
        // if there are no defined results in the Input Type, we do not validate the Results - only Cross Validator will check it with Dispatcher 
        if (expectedResultTypes.Any())
        {
            //compare types found in Handler Generic Type with method return types 
            MethodResultTypesValidator.Validate(handleMethod, expectedResultTypes, handlerType, handlerType);
        }
    }

    private static Type[] GetResultTypes(Type[] genericArguments)
    {
        //24.06.2023 - Since the first generic argument is the input type (InputType), it is disregarded when obtaining the result types
        return genericArguments.Length < 1
            ? Array.Empty<Type>()
            : genericArguments.Skip(1).Take(genericArguments.Length - 1).ToArray();
    }
}