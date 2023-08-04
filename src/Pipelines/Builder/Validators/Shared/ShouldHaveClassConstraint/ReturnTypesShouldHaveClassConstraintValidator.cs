using Pipelines.Builder.Validators.Shared.ShouldHaveClassConstraint.Exceptions;
using Pipelines.Utils;

namespace Pipelines.Builder.Validators.Shared.ShouldHaveClassConstraint;

internal static class ReturnTypesShouldHaveClassConstraintValidator
{
    internal static void Validate(List<Type> types, Type type)
    {
        foreach (var typeToValidate in types)
        {
            if (!GenericTypeHasClassConstraint.Check(typeToValidate))
            {
                throw new ReturnTypesShouldHaveClassConstraintException(typeToValidate, type);
            }
        }
    }
}