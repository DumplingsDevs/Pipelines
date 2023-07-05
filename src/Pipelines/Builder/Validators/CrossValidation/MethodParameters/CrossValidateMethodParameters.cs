using Pipelines.Utils;

namespace Pipelines.Builder.Validators.CrossValidation.MethodParameters;

public static class CrossValidateMethodParameters
{
    public static void Validate(Type handlerType, Type dispatcher)
    {
        var handlerMethodParameters = GetMethodParameters(handlerType);
        var dispatcherMethodParameters = GetMethodParameters(dispatcher);

        if (handlerMethodParameters.Count != dispatcherMethodParameters.Count)
        {
            throw new Exception();
        }
        
        //TO DO ADD dedicated validation for first parameter
        // or maybe even we shouldn't validate first parameters as there are dedicated validators that check InputTypes
        //for both Dispatcher and Handler

        for (var i = 0; i < handlerMethodParameters.Count; i++)
        {
            var handlerParam = handlerMethodParameters[i];
            var dispatcherParam = dispatcherMethodParameters[i];
            
            if (TypeNamespaceComparer.Compare(handlerParam, dispatcherParam))
            {
                //Method Parameters Mismatch exception
                throw new Exception();
            }
        }
    }

    private static List<Type> GetMethodParameters(Type type)
    {
        return type.GetMethods().First().GetParameters().Select(x=> x.ParameterType).ToList();
    }
}