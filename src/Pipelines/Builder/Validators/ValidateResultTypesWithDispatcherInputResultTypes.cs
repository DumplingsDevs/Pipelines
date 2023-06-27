using Pipelines.Utils;

namespace Pipelines.Builder.Validators;

internal static class ValidateResultTypesWithDispatcherInputResultTypes
{
    internal static void Validate(Type inputType, Type handlerType)
    {
        var expectedResultTypes = inputType.GetGenericArguments();
        var handleMethod = handlerType.GetMethods().First();
        
        MethodResultTypesValidator.Validate(handleMethod, expectedResultTypes, handlerType);
    }
}