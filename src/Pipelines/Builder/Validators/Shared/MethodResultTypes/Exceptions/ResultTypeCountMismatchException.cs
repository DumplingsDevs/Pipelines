namespace Pipelines.Builder.Validators.Shared.MethodResultTypes.Exceptions;

public class ResultTypeCountMismatchException : Exception
{
    private const string ErrorMessageFormat = "Expected {0} result type(s), but found {1}";

    public ResultTypeCountMismatchException(int expected, int found) 
        : base(string.Format(ErrorMessageFormat, expected, found)) { }
}