namespace Pipelines.Builder.Validators.Shared.MethodResultTypes.Exceptions;

internal class ExpectedVoidMethodException : Exception
{
    private const string ErrorMessage =
        "Expected void method  based on source {0}, but found method with result in type: {1}";

    internal ExpectedVoidMethodException(Type type, Type expectedResultSourceType) : base(string.Format(ErrorMessage,
        expectedResultSourceType, type))
    {
    }
}