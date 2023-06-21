namespace Pipelines.Exceptions;

internal class HandlerNotRegisteredException : Exception
{
    private const string ErrorMessage = "Handler is not registered";

    public HandlerNotRegisteredException() : base(ErrorMessage)
    {
    }
}