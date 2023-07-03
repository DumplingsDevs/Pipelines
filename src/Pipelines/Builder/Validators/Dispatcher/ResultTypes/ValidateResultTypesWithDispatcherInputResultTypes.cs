using Pipelines.Builder.Validators.Shared.MethodResultTypes;

namespace Pipelines.Builder.Validators.Dispatcher.ResultTypes;

internal static class ValidateResultTypesWithDispatcherInputResultTypes
{
    internal static void Validate(Type inputType, Type handlerType)
    {
        var expectedResultTypes = inputType.GetGenericArguments();
        var handleMethod = handlerType.GetMethods().First();
        
        MethodResultTypesValidator.Validate(handleMethod, expectedResultTypes, handlerType);
        
        // var genericMethodTypes = handleMethod.GetGenericArguments();
        // //compare types found in method generic arguments (Handler<TResult>) with method return types 
        // MethodResultTypesValidator.Validate(handleMethod, genericMethodTypes, handlerType);
    }
}