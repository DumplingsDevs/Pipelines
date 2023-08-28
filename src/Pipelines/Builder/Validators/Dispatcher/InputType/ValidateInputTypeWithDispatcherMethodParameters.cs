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

        var inputTypeMatch = firstMethodParameterType.IsGenericMethodParameter
            ? TryFindInputType(inputType, firstMethodParameterType)
            : TypeNamespaceComparer.Compare(inputType, firstMethodParameterType);

        if (!inputTypeMatch)
        {
            throw new DispatcherMethodInputTypeMismatchException(inputType, firstMethodParameterType);
        }
    }

    private static bool TryFindInputType(Type inputType, Type firstMethodParameterType)
    {
        var inputTypeFound = false;
        foreach (var type in firstMethodParameterType.GetGenericParameterConstraints())
        {
            inputTypeFound = TypeNamespaceComparer.Compare(inputType, type);
            if (inputTypeFound)
            {
                break;
            }
        }

        return inputTypeFound;
    }

    private static Type GetFirstMethodParameterType(Type dispatcherType)
    {
        var handleMethod = dispatcherType.GetMethods().First();
        var dispatcherInputType = handleMethod.GetParameters().First().ParameterType;
        return dispatcherInputType;
    }
}