namespace Pipelines.Builder.Validators.Shared.MethodResultTypes.Exceptions;

internal class ExpectedVoidMethodException : Exception
{
    private const string ErrorMessage = "Expected void method, but found method with result in type: {0}";

    public ExpectedVoidMethodException(Type type) : base(string.Format(ErrorMessage, type))
    {
    }
}