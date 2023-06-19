namespace Pipelines.Builder.Validators;

internal static class ValidateHandleMethodInHandler
{
    //var z = type.GetGenericArguments().First().GetGenericParameterConstraints().First();

    //remember to validate if inputType have generic type. Return value should be same.

    internal static void Validate(Type inputType, Type handlerType)
    {
        var genericArguments = handlerType.GetGenericArguments();

        switch (genericArguments.Length)
        {
            case 0:
                throw new Exception();
        }

        if (InputTypeNotMatchFirstGenericParameterConstraint(inputType, genericArguments.First()))
        {
            throw new Exception();
        }

        var methods = handlerType
            .GetMethods()
            .Where(x => x.GetParameters()
                .Any(y => y.ParameterType == inputType))
            .ToList();
    }

    private static bool InputTypeNotMatchFirstGenericParameterConstraint(Type inputType, Type handlerGenericParameter)
    {
        var handlerInputConstraint = handlerGenericParameter.GetGenericParameterConstraints();
        //GenericTypeArguments
        if (handlerInputConstraint.Length is 0 or > 2)
        {
            throw new Exception();
        }

        var handleInputType = handlerInputConstraint.First();

        //Its enough to validate namespaces + GenericTypeArguments. It will check if:
        //handler use expected input (due to namespaces matches)
        //within same namespace, type with same generic argument lenght can not exists (build error) 
        ValidateNamespaces();
        ValidateGenericTypeArgumentsLenght(inputType, handleInputType);

        return false;

        void ValidateNamespaces()
        {
            var handleInputNamespace = handleInputType.Namespace + "." + handleInputType.Name;
            if (!handleInputNamespace.Equals(inputType.FullName))
            {
                throw new Exception();
            }
        }
    }

    private static void ValidateGenericTypeArgumentsLenght(Type inputType, Type handleInputType)
    {
        var inputGenericArguments = inputType.GetGenericArguments();
        var handleGenericArguments = handleInputType.GetGenericArguments();

        if (inputGenericArguments.Length != handleGenericArguments.Length)
        {
            throw new Exception();
        }
    }
}