using Pipelines.Builder.Validators.Handler.InputType.Exceptions;
using Pipelines.Utils;

namespace Pipelines.Builder.Validators.Handler.InputType;

internal static class ValidateInputTypeWithHandlerGenericArguments
{
    internal static void Validate(Type inputType, Type handlerType)
    {
        ParamValidator.NotNull(inputType, nameof(inputType));
        ParamValidator.NotNull(handlerType, nameof(handlerType));

        var genericArguments = handlerType.GetGenericArguments();
        if (!genericArguments.Any())
        {
            throw new GenericArgumentsNotFoundException(handlerType);
        }

        if (InputTypeNotMatchGenericArgumentConstraint(inputType, genericArguments.First()))
        {
            throw new HandlerInputTypeMismatchException(inputType, genericArguments.First());
        }
    }

    //to do refactor this method
    private static bool InputTypeNotMatchGenericArgumentConstraint(Type inputType, Type handlerGenericParameter)
    {
        //Only one constraint is expected for 
        var handlerInputConstraints = handlerGenericParameter.GetGenericParameterConstraints();
        if (handlerInputConstraints.Length == 0)
        {
            throw new InvalidConstraintLengthException(handlerGenericParameter);
        }
        
        var inputTypeFound = false;
        
        foreach (var handlerInputConstraint in handlerInputConstraints)
        {
            // Validating both the namespaces and the GenericTypeArguments is sufficient in this case.
            // This approach ensures the following checks:
            // 1. Within the same namespace, a type with the same generic argument length cannot exist as it would result in a build error.
            // 2. The handler uses the expected input type as defined by matching namespaces.
            inputTypeFound = TypeNamespaceComparer.Compare(handlerInputConstraint, inputType);
            if (inputTypeFound)
            {
                break;
            }
        }
        
        return !inputTypeFound;
    }
}