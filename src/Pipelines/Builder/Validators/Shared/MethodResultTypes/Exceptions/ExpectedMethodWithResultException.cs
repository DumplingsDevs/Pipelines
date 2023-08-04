namespace Pipelines.Builder.Validators.Shared.MethodResultTypes.Exceptions;

internal class ExpectedMethodWithResultException : Exception
{
    private const string ErrorMessage = "Expected method with result, but method was void in type: ";

    public ExpectedMethodWithResultException(Type type) : base(ErrorMessage + type.Name) { }
}