namespace Pipelines.Exceptions;

internal class NamespaceMismatchException : Exception
{
    private const string ErrorMessageFormat = "Namespace mismatch. Expected {0}, but found {1}";

    internal NamespaceMismatchException(string expected, string found) 
        : base(string.Format(ErrorMessageFormat, expected, found)) { }
}