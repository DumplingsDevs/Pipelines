namespace Pipelines.Builder.Validators.Shared.MethodResultTypes.Exceptions;

internal class ExpectedMethodWithResultException : Exception
{
    private const string ErrorMessage = "Expected method with result, but method was void in type: {0}";

    public ExpectedMethodWithResultException(Type type) : base(string.Format(ErrorMessage, type)) { }
}