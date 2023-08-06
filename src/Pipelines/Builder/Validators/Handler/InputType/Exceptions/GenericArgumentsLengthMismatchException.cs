namespace Pipelines.Builder.Validators.Handler.InputType.Exceptions;

internal class GenericArgumentsLengthMismatchException : Exception
{
    private const string ErrorMessageFormat = "Mismatch in length of generic arguments. Expected {0}, but found {1}";

    internal GenericArgumentsLengthMismatchException(int expected, int found) 
        : base(string.Format(ErrorMessageFormat, expected, found)) { }
}