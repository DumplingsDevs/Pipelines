namespace Pipelines.Builder.Validators.Shared.OnlyOneHandleMethod.Exceptions;

internal class HandlerMethodNotFoundException : Exception
{
    private const string ErrorMessage = "Cannot find method in {0} which accepts input of type {1}";

    public HandlerMethodNotFoundException(Type handlerType, Type inputType) : base(
        string.Format(ErrorMessage, handlerType.Name, inputType.Name))
    {
    }
}