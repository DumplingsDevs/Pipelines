using System.Reflection;
using Pipelines.Builder.Validators.CrossValidation.MethodParameters.Exceptions;
using Pipelines.Utils;

namespace Pipelines.Builder.Validators.CrossValidation.MethodParameters;

internal static class CrossValidateMethodParameters
{
    internal static void Validate(Type handlerType, Type dispatcherType, MethodInfo handlerHandleMethod,
        MethodInfo dispatcherHandleMethod)
    {
        var handlerMethodParameters = GetMethodParameters(handlerType, handlerHandleMethod);
        var dispatcherMethodParameters = GetMethodParameters(dispatcherType, dispatcherHandleMethod);

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

    private static List<Type> GetMethodParameters(Type type, MethodInfo methodInfo)
    {
        return type.GetMethods().First(x => x.Equals(methodInfo))
            .GetParameters().Select(x => x.ParameterType).ToList();
    }
}