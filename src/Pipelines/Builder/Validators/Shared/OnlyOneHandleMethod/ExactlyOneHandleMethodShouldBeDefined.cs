using Pipelines.Builder.Validators.Shared.OnlyOneHandleMethod.Exceptions;
using Pipelines.Utils;

namespace Pipelines.Builder.Validators.Shared.OnlyOneHandleMethod;

internal static class ExactlyOneHandleMethodShouldBeDefined
{
    internal static void Validate(Type inputType, params Type[] typesToValidate)
    {
        ParamValidator.NotNull(inputType, nameof(inputType));
        ParamValidator.NotNullOrEmpty(typesToValidate, nameof(typesToValidate));

        foreach (var type in typesToValidate)
        {
            var methods = type
                .GetMethods()
                .Where(x => x.IsPublic).ToList();

            switch (methods.Count)
            {
                case 0:
                    throw new HandlerMethodNotFoundException(type.Name, inputType.Name);
                case > 1:
                    throw new MultipleHandlerMethodsException(inputType.Name);
            }
        }
    }
}