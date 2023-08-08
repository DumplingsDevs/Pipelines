namespace Pipelines.Builder.Validators.Shared.MethodResultTypes.Exceptions;

public class ExpectedMethodWithResultException : Exception
{
    private const string ErrorMessage =
        "Expected method with result based on source {0}, but method was void in type: {1}";

    internal ExpectedMethodWithResultException(Type type, Type expectedResultSourceType) : base(
        string.Format(ErrorMessage, expectedResultSourceType, type))
    {
    }
}