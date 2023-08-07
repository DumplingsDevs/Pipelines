namespace Pipelines.Exceptions;

public class HandlerNotRegisteredException : Exception
{
    private const string ErrorMessage = "Handler {} is not registered";

    public HandlerNotRegisteredException(Type handlerTypeWithInput) : base(string.Format(ErrorMessage,
        handlerTypeWithInput.Namespace))
    {
    }
}