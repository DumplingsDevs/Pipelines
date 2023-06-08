namespace Pipelines.Exceptions;

internal class HandlerMethodNotImplementedException : Exception
{
    private const string ErrorMessage = "Cannot find method in {0} which accepts input of type {1}";

    public HandlerMethodNotImplementedException(string handlerTypeName, string inputTypeName) : base(
        string.Format(ErrorMessage, handlerTypeName, inputTypeName))
    {
    }
}