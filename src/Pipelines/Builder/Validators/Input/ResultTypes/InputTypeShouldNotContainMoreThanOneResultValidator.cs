using Pipelines.Builder.Validators.Input.ResultTypes.Exceptions;

namespace Pipelines.Builder.Validators.Input.ResultTypes;

internal static class InputTypeShouldNotContainMoreThanOneResult
{
    internal static void Validate(Type inputType)
    {
        var expectedResultTypes = inputType.GetGenericArguments();

        if (expectedResultTypes.Length > 1)
        {
            throw new InputTypeShouldNotContainMoreThanOneResultException(expectedResultTypes.Length, inputType);
        }
    }
}