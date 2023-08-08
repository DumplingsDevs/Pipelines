namespace Pipelines.Builder.Validators.Shared.OnlyOneHandleMethod.Exceptions;

public class HandlerMethodNotFoundException : Exception
{
    private const string ErrorMessage = "Cannot find method in {0} which accepts input of type {1}";

    internal HandlerMethodNotFoundException(Type handlerType, Type inputType) : base(
        string.Format(ErrorMessage, handlerType.Name, inputType.Name))
    {
    }
}