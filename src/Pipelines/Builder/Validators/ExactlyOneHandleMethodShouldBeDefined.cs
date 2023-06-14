using Pipelines.Exceptions;

namespace Pipelines.Builder.Validators;

internal static class ExactlyOneHandleMethodShouldBeDefined
{
    internal static void Validate(Type inputType, params Type[] typesToValidate)
    {
        foreach (var type in typesToValidate)
        {
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