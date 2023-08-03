using Pipelines.Builder.Validators.Input.ResultTypes.Exceptions;
using Pipelines.Builder.Validators.Shared.ShouldHaveClassConstraint;

namespace Pipelines.Builder.Validators.Input.ResultTypes;

internal static class ValidateInputReturnTypes
{
    internal static void Validate(Type inputType)
    {
        var expectedResultTypes = inputType.GetGenericArguments();

        if (expectedResultTypes.Length > 1)
        {
            throw new InputTypeShouldNotContainMoreThanOneResultException(expectedResultTypes.Length, inputType);
        }
        
        ReturnTypesShouldHaveClassConstraintValidator.Validate(expectedResultTypes.ToList(), inputType);
    }
}