using Pipelines.Exceptions;
using Pipelines.Utils;

namespace Pipelines.Builder.Validators.Shared.InterfaceConstraint;

internal static class AllProvidedTypesShouldBeInterface
{
    internal static void Validate(params Type[] typesToValidate)
    {
        ParamValidator.NotNullOrEmpty(typesToValidate, nameof(typesToValidate));

        foreach (var type in typesToValidate)
        {
            if (!type.IsInterface)
            {
                throw new InvalidTypeException(type.Name);
            }
        }
    }
}