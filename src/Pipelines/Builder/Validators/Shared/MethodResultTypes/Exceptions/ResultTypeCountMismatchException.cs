namespace Pipelines.Builder.Validators.Shared.MethodResultTypes.Exceptions;

public class ResultTypeCountMismatchException : Exception
{
    private const string ErrorMessageFormat =
        "Expected '{0}' result type(s) from source '{1}', but found '{2}' in source '{3}'";

    internal ResultTypeCountMismatchException(int expectedResultTypesLength, int methodReturnTypesLength,
        Type resultSourceType, Type expectedResultSourceType)
        : base(string.Format(ErrorMessageFormat, expectedResultTypesLength, expectedResultSourceType,
            methodReturnTypesLength, resultSourceType))
    {
    }
}