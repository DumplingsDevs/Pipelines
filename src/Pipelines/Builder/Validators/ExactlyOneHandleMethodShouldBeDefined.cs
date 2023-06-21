using Pipelines.Exceptions;

namespace Pipelines.Builder.Validators;

public static class ExactlyOneHandleMethodShouldBeDefined
{
    internal static void Validate(Type inputType, params Type[] typesToValidate)
    {
        foreach (var type in typesToValidate)
        {
            var methods = type
                .GetMethods()
                .Where(x => x.IsPublic).ToList();

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