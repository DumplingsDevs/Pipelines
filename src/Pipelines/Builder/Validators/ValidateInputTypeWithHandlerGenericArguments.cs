using Pipelines.Exceptions;
using Pipelines.Utils;

namespace Pipelines.Builder.Validators;

internal static class ValidateInputTypeWithHandlerGenericArguments
{
    internal static void Validate(Type inputType, Type handlerType)
    {
        ParamValidator.NotNull(inputType, nameof(inputType));
        ParamValidator.NotNull(handlerType, nameof(handlerType));
        
        var genericArguments = handlerType.GetGenericArguments();

        switch (genericArguments.Length)
        {
            case 0:
                throw new GenericArgumentsNotFoundException(handlerType);
        }

        if (InputTypeNotMatchGenericArgumentConstraint(inputType, genericArguments.First()))
        {
            throw new InputTypeMismatchException(inputType, genericArguments.First());
        }
    }

    private static bool InputTypeNotMatchGenericArgumentConstraint(Type inputType, Type handlerGenericParameter)
    {
        //Only one constraint is expected for 
        var handlerInputConstraint = handlerGenericParameter.GetGenericParameterConstraints();
        if (handlerInputConstraint.Length is not 1)
        {
            throw new InvalidConstraintLengthException(handlerGenericParameter);
        }

        var handleInputType = handlerInputConstraint.First();

        // Validating both the namespaces and the GenericTypeArguments is sufficient in this case.
        // This approach ensures the following checks:
        // 1. Within the same namespace, a type with the same generic argument length cannot exist as it would result in a build error.
        // 2. The handler uses the expected input type as defined by matching namespaces.
        ValidateGenericTypeArgumentsLenght(inputType, handleInputType);
        TypeNamespaceValidator.Validate(handleInputType,inputType);

        return false;
    }

    private static void ValidateGenericTypeArgumentsLenght(Type inputType, Type handleInputType)
    {
        var inputGenericArguments = inputType.GetGenericArguments();
        var handleGenericArguments = handleInputType.GetGenericArguments();

        if (inputGenericArguments.Length != handleGenericArguments.Length)
        {
            throw new GenericArgumentsLengthMismatchException(inputGenericArguments.Length, handleGenericArguments.Length);
        }
    }
}