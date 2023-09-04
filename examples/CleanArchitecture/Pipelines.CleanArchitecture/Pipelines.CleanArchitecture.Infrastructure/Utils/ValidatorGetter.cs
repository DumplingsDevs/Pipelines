using FluentValidation;
using Pipelines.CleanArchitecture.Infrastructure.Utils.Exceptions;

namespace Pipelines.CleanArchitecture.Infrastructure.Utils;

internal static class ValidatorGetter
{
    private static dynamic? GetValidator(IServiceProvider provider, Type queryType, Type resultType, string typeName,
        Type type)
    {
        var queryAndResultTypes = new[] { queryType, resultType };
        Type? validatorType = null;

        try
        {
            validatorType = type.MakeGenericType(queryAndResultTypes);
        }
        catch (Exception e)
        {
            throw new UnableToConstructGenericTypeException(typeName, e);
        }

        if (validatorType == null)
        {
            throw new UnableToConstructGenericTypeException(typeName);
        }

        dynamic? validator = provider.GetService(validatorType);
        return validator;
    }
    
    public static dynamic? GetFluentQueryValidator(IServiceProvider provider, Type queryType,
        string typeName)
    {
        var type = typeof(IValidator<>);

        var validator = GetFluentValidator(provider, queryType, typeName, type);

        return validator;
    }
    
    private static dynamic? GetFluentValidator(IServiceProvider provider, Type queryType, string typeName,
        Type type)
    {
        Type? validatorType;
        
        try
        {
            validatorType = type.MakeGenericType(queryType);
        }
        catch (Exception e)
        {
            throw new UnableToConstructGenericTypeException(typeName, e);
        }

        if (validatorType == null)
        {
            throw new UnableToConstructGenericTypeException(typeName);
        }

        dynamic? validator = provider.GetService(validatorType);
        return validator;
    }
}