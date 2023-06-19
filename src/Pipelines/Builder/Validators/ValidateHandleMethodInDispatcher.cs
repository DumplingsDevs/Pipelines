using Pipelines.Exceptions;

namespace Pipelines.Builder.Validators;

internal static class ValidateHandleMethodInDispatcher
{
    internal static void Validate(Type inputType, params Type[] typesToValidate)
    {
        foreach (var type in typesToValidate)
        {
            var z = type.GetGenericArguments().First().GetGenericParameterConstraints().First();
            if (z.Namespace +"." + z.Name == inputType.FullName)
            {
                
            }
            var methods = type
                .GetMethods()
                .Where(x => x.GetParameters()
                    .Any(y => y.ParameterType == inputType))
                .ToList();

            switch (methods.Count)
            {
                case 0:
                    throw new HandlerMethodNotImplementedException(type.Name, inputType.Name);
                case > 1:
                    throw new MultipleHandlerMethodsException(inputType.Name);
            }
        }
    }
}