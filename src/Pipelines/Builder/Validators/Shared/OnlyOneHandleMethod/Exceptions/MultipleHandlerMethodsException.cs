namespace Pipelines.Builder.Validators.Shared.OnlyOneHandleMethod.Exceptions;

public class MultipleHandlerMethodsException : Exception
{
    private const string ErrorMessage = "Handler should implement only one method with input of type {0}";

    internal MultipleHandlerMethodsException(Type inputType) : base(string.Format(ErrorMessage, inputType.Name))
    {
        
    }
}