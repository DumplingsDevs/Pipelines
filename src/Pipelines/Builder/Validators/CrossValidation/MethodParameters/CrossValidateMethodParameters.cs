using Pipelines.Builder.Validators.CrossValidation.MethodParameters.Exceptions;
using Pipelines.Utils;

namespace Pipelines.Builder.Validators.CrossValidation.MethodParameters;

public static class CrossValidateMethodParameters
{
    public static void Validate(Type handlerType, Type dispatcherType)
    {
        var handlerMethodParameters = GetMethodParameters(handlerType);
        var dispatcherMethodParameters = GetMethodParameters(dispatcherType);

        if (handlerMethodParameters.Count != dispatcherMethodParameters.Count)
        {
            throw new ParameterCountMismatchException(handlerMethodParameters.Count, dispatcherMethodParameters.Count);
        }
        
        for (var i = 1; i < handlerMethodParameters.Count; i++)
        {
            var handlerParam = handlerMethodParameters[i];
            var dispatcherParam = dispatcherMethodParameters[i];
            
            if (!TypeNamespaceComparer.Compare(handlerParam, dispatcherParam))
            {
                throw new ParameterTypeMismatchException(handlerParam, dispatcherParam, i);
            }
        }
    }

    private static List<Type> GetMethodParameters(Type type)
    {
        return type.GetMethods().First().GetParameters().Select(x=> x.ParameterType).ToList();
    }
}