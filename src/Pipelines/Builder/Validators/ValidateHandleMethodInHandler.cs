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
            case > 2:
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
        var inputConstraint = handlerGenericParameter.GetGenericParameterConstraints();
        
        if (inputConstraint.Length == 0)
        {
            throw new Exception();
        }
        
        var handleInputNamespace = handlerGenericParameter.Namespace + "." + handlerGenericParameter.Name;
        
        if (!handleInputNamespace.Equals(inputType.FullName))
        {
            throw new Exception();
        }

        return false;
    }
}