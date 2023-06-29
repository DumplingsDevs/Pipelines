namespace Pipelines.Builder.Validators.Shared.MethodResultTypes.Exceptions;

public class ReturnTypeMismatchException : Exception
{
    private const string ErrorMessageFormat = "Expected return type {0}, but found {1}";

    public ReturnTypeMismatchException(Type expected, Type found) 
        : base(string.Format(ErrorMessageFormat, expected.Name, found.Name)) { }
}