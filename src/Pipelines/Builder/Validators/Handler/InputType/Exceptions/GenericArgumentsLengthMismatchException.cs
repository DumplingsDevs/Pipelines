namespace Pipelines.Builder.Validators.Handler.InputType.Exceptions;

public class GenericArgumentsLengthMismatchException : Exception
{
    private const string ErrorMessageFormat = "Mismatch in length of generic arguments. Expected {0}, but found {1}";

    public GenericArgumentsLengthMismatchException(int expected, int found) 
        : base(string.Format(ErrorMessageFormat, expected, found)) { }
}