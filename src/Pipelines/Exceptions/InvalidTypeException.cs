namespace Pipelines.Exceptions;

internal class InvalidTypeException : Exception
{
    private const string ErrorMessage = "The provide type '{0}' is not interface.";

    internal InvalidTypeException(string typeName) : base(string.Format(ErrorMessage, typeName))
    {
    }
}