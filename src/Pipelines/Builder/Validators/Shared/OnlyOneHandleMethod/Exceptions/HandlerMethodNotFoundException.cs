namespace Pipelines.Builder.Validators.Shared.OnlyOneHandleMethod.Exceptions;

internal class HandlerMethodNotFoundException : Exception
{
    private const string ErrorMessage = "Cannot find method in {0} which accepts input of type {1}";

    public HandlerMethodNotFoundException(string handlerTypeName, string inputTypeName) : base(
        string.Format(ErrorMessage, handlerTypeName, inputTypeName))
    {
    }
}