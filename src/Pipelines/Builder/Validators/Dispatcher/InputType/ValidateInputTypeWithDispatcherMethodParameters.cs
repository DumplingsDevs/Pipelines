using Pipelines.Builder.Validators.Dispatcher.InputType.Exceptions;
using Pipelines.Utils;

namespace Pipelines.Builder.Validators.Dispatcher.InputType;

internal static class ValidateInputTypeWithDispatcherMethodParameters
{
    internal static void Validate(Type inputType, Type dispatcherType)
    {
        ParamValidator.NotNull(inputType, nameof(inputType));
        ParamValidator.NotNull(dispatcherType, nameof(dispatcherType));

        var firstMethodParameterType = GetFirstMethodParameterType(dispatcherType);

        var areEqual = TypeNamespaceComparer.Compare(inputType, firstMethodParameterType);
        if (!areEqual)
        {
            throw new DispatcherMethodInputTypeMismatchException(inputType, firstMethodParameterType);
        }
    }

    private static Type GetFirstMethodParameterType(Type dispatcherType)
    {
        var handleMethod = dispatcherType.GetMethods().First();
        var dispatcherInputType = handleMethod.GetParameters().First().ParameterType;
        return dispatcherInputType;
    }
}