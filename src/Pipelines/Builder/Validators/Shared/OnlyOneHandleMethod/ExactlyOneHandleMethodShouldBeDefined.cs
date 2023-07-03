using Pipelines.Builder.Validators.Shared.OnlyOneHandleMethod.Exceptions;
using Pipelines.Utils;

namespace Pipelines.Builder.Validators.Shared.OnlyOneHandleMethod;

internal static class ExactlyOneHandleMethodShouldBeDefined
{
    internal static void Validate(Type inputType, Type typeToValidate)
    {
        ParamValidator.NotNull(inputType, nameof(inputType));
        ParamValidator.NotNull(typeToValidate, nameof(typeToValidate));

        var methods = typeToValidate
            .GetMethods()
            .Where(x => x.IsPublic).ToList();

        switch (methods.Count)
        {
            case 0:
                throw new HandlerMethodNotFoundException(typeToValidate, inputType);
            case > 1:
                throw new MultipleHandlerMethodsException(inputType);
        }
    }
}