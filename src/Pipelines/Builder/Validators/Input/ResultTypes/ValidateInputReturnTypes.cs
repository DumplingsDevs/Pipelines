using Pipelines.Builder.Validators.Shared.ShouldHaveClassConstraint;

namespace Pipelines.Builder.Validators.Input.ResultTypes;

internal static class ValidateInputReturnTypes
{
    internal static void Validate(Type inputType)
    {
        var expectedResultTypes = inputType.GetGenericArguments();
        
        ReturnTypesShouldHaveClassConstraintValidator.Validate(expectedResultTypes.ToList(), inputType);
    }
}