using Pipelines.Builder.Validators.Shared.MethodResultTypes;

namespace Pipelines.Builder.Validators.Dispatcher.ResultTypes;

internal static class ValidateResultTypesWithDispatcherInputResultTypes
{
    internal static void Validate(Type inputType, Type handlerType)
    {
        var expectedResultTypes = inputType.GetGenericArguments();
        var handleMethod = handlerType.GetMethods().First();

        //compare types found in inputType generic arguments (IICommand<TResult>) with method return types (Task<TResult>)
        MethodResultTypesValidator.Validate(handleMethod, expectedResultTypes, handlerType, inputType);

        var genericMethodTypes = handleMethod.GetGenericArguments();

        //compare types found in method generic arguments (Handler<TResult>) with method return types (Task<TResult>)
        MethodResultTypesValidator.Validate(handleMethod, genericMethodTypes, handlerType, handlerType);
    }
}