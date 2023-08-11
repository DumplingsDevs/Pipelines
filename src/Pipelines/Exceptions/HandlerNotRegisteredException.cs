namespace Pipelines.Exceptions;

public class HandlerNotRegisteredException : Exception
{
    private const string ErrorMessage = "Handler {0} is not registered";

    internal HandlerNotRegisteredException(Type handlerTypeWithInput) : base(string.Format(ErrorMessage,
        handlerTypeWithInput.FullName))
    {
    }
}