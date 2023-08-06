namespace Pipelines.Builder.Validators.Handler.InputType.Exceptions;

internal class HandlerInputTypeMismatchException : Exception
{
    private const string ErrorMessageFormat = "Input type {0} does not match handler's generic argument constraint {1}";

    internal HandlerInputTypeMismatchException(Type inputType, Type handlerGenericParameter) 
        : base(string.Format(ErrorMessageFormat, inputType.Name, handlerGenericParameter.Name)) { }
}