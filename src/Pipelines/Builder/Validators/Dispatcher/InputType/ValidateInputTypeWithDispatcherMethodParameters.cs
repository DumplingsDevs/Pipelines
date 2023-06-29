using Pipelines.Utils;

namespace Pipelines.Builder.Validators.Dispatcher.InputType;

internal static class ValidateInputTypeWithDispatcherMethodParameters
{
    internal static void Validate(Type inputType, Type dispatcherType)
    {
        ParamValidator.NotNull(inputType, nameof(inputType));
        ParamValidator.NotNull(dispatcherType, nameof(dispatcherType));

        var handleMethod = dispatcherType.GetMethods().First();
        var dispatcherInputType = handleMethod.GetParameters().First().ParameterType;

        var areEqual = TypeNamespaceComparer.Compare(inputType, dispatcherInputType);
        if (!areEqual)
        {
            throw new Exception();
        }
    }
}