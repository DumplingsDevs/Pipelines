using Pipelines.Exceptions;

namespace Pipelines.Builder.Validators;

public class ExactlyOneHandleMethodShouldBeDefined
{
    public static void Validate(Type inputType, params Type[] typesToValidate)
    {
        foreach (var type in typesToValidate)
        {
            var methods = type.GetType()
                .GetMethods()
                .Where(x => x.GetParameters()
                    .Any(y => y.ParameterType == inputType))
                .ToList();

            switch (methods.Count)
            {
                case 0:
                    throw new HandlerMethodNotImplementedException(type.GetType().Name, inputType.Name);
                case > 1:
                    throw new MultipleHandlerMethodsException(inputType.Name);
            }
        }
    }
}