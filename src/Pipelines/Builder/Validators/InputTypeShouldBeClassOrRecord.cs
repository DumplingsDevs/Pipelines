using Pipelines.Exceptions;

namespace Pipelines.Builder.Validators;

internal static class InputTypeShouldBeClassOrRecord
{
    internal static void Validate(Type inputType)
    {
        if (!inputType.IsClass || inputType.IsValueType || inputType.IsEnum || IsStringType(inputType))
        {
            throw new InvalidInputTypeException();
        }
    }

    private static bool IsStringType(Type inputType)
    {
        return inputType == typeof(string) || inputType == typeof(String);
    }
}