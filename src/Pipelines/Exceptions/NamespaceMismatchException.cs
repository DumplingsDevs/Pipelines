namespace Pipelines.Exceptions;

public class NamespaceMismatchException : Exception
{
    private const string ErrorMessageFormat = "Namespace mismatch. Expected {0}, but found {1}";

    public NamespaceMismatchException(string expected, string found) 
        : base(string.Format(ErrorMessageFormat, expected, found)) { }
}