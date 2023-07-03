namespace Pipelines.Builder.Validators.Shared.OnlyOneHandleMethod.Exceptions;

internal class MultipleHandlerMethodsException : Exception
{
    private const string ErrorMessage = "Handler should implement only one method with input of type {0}";

    public MultipleHandlerMethodsException(Type inputType) : base(string.Format(ErrorMessage, inputType.Name))
    {
        
    }
}