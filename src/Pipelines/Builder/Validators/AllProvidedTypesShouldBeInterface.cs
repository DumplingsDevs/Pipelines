using Pipelines.Exceptions;

namespace Pipelines.Builder.Validators;

internal static class AllProvidedTypesShouldBeInterface
{
    internal static void Validate(params Type[] typesToValidate)
    {
        foreach (var type in typesToValidate)
        {
            if (!type.IsInterface)
            {
                throw new InvalidTypeException(type.Name);
            }
        }
    }
}