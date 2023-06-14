namespace Pipelines.Exceptions;

internal class InvalidInputTypeException : Exception
{
    private const string ErrorMessage = "The Input Type is not of type Record or class.";

    internal InvalidInputTypeException() : base(ErrorMessage)
    {
    }
}