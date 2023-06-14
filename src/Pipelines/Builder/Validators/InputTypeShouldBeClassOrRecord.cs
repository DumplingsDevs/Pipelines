using Pipelines.Exceptions;

namespace Pipelines.Builder.Validators;

internal static class InputTypeShouldBeClassOrRecord
{
    internal static void Validate(Type inputType)
    {
        if (!inputType.IsClass || inputType.IsValueType && !inputType.IsEnum)
        {
            throw new InvalidInputTypeException();
        }
    }
}