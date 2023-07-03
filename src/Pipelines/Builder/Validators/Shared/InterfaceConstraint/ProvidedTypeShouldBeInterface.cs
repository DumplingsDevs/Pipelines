using Pipelines.Builder.Validators.Shared.InterfaceConstraint.Exceptions;
using Pipelines.Utils;

namespace Pipelines.Builder.Validators.Shared.InterfaceConstraint;

internal static class ProvidedTypeShouldBeInterface
{
    internal static void Validate(Type typeToValidate)
    {
        ParamValidator.NotNull(typeToValidate, nameof(typeToValidate));

        if (!typeToValidate.IsInterface)
        {
            throw new ProvidedTypeIsNotInterfaceException(typeToValidate);
        }
    }
}