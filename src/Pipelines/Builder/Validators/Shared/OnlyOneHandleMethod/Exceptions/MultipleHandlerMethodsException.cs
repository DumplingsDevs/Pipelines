namespace Pipelines.Builder.Validators.Shared.OnlyOneHandleMethod.Exceptions;

internal class MultipleHandlerMethodsException : Exception
{
    private const string ErrorMessage = "Handler should implement only one method with input of type {0}";

    public MultipleHandlerMethodsException(string inputTypeName) : base(string.Format(ErrorMessage, inputTypeName))
    {
        
    }
}