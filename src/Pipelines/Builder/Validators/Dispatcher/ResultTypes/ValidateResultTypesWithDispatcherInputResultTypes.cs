using Pipelines.Builder.Validators.Shared.MethodResultTypes;

namespace Pipelines.Builder.Validators.Dispatcher.ResultTypes;

internal static class ValidateResultTypesWithDispatcherInputResultTypes
{
    internal static void Validate(Type inputType, Type handlerType)
    {
        var expectedResultTypesFromInputType = inputType.GetGenericArguments();
        var handleMethod = handlerType.GetMethods().First();
        var genericMethodTypes = handleMethod.GetGenericArguments();

        var isThereIsAnyExpectedResultsFromInput = expectedResultTypesFromInputType.Any();
        var isThereIsAnyExpectedResultsInHandleMethod = genericMethodTypes.Any();

        // if there are no defined results in the Input Type, we do not validate the Results - only Cross Validator will consistency with Handler 
        if (isThereIsAnyExpectedResultsFromInput)
        {
            //compare types found in inputType generic arguments (IICommand<TResult>) with method return types (Task<TResult>)
            MethodResultTypesValidator.Validate(handleMethod, expectedResultTypesFromInputType, handlerType, inputType);
        }

        // if there are no defined generic results in the Handler method (like, Handler<TResult>) - then we do not validate the Results - only Cross Validator will consistency with Handler 
        if (isThereIsAnyExpectedResultsFromInput && isThereIsAnyExpectedResultsInHandleMethod)
        {
            //compare types found in method generic arguments (Handler<TResult>) with method return types (Task<TResult>)
            MethodResultTypesValidator.Validate(handleMethod, genericMethodTypes, handlerType, handlerType);
        }
    }
}