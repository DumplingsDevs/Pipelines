using Pipelines.Builder.Validators.Shared.ShouldHaveClassConstraint.Exceptions;
using Pipelines.Utils;

namespace Pipelines.Builder.Validators.Shared.ShouldHaveClassConstraint;

internal static class ReturnTypesShouldBeClassOrHaveClassConstraintValidator
{
    internal static void Validate(List<Type> types, Type sourceType)
    {
        foreach (var typeToValidate in types)
        {
            var baseTypeConstraint = typeToValidate.IsGenericParameter
                ? typeToValidate.GetGenericParameterConstraints().FirstOrDefault()
                : null;
            
            var isClass = (baseTypeConstraint != null && baseTypeConstraint != typeof(object));

            var hasClassConstraint = GenericTypeHasClassConstraint.Check(typeToValidate);

            if (!isClass && !hasClassConstraint)
            {
                throw new ReturnTypesShouldHaveClassConstraintException(typeToValidate, sourceType);
            }
        }
    }
}