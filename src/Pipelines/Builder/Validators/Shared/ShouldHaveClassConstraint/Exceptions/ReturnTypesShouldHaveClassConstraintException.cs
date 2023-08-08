namespace Pipelines.Builder.Validators.Shared.ShouldHaveClassConstraint.Exceptions;

public class ReturnTypesShouldHaveClassConstraintException : Exception
{
    private const string ErrorMessage = "Return type {0} in type {1} does not contain class constraint";

    internal ReturnTypesShouldHaveClassConstraintException(Type resultType, Type type) : base(string.Format(
        ErrorMessage,
        resultType, type))
    { }
}