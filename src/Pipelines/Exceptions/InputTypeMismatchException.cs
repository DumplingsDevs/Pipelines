namespace Pipelines.Exceptions;

public class InputTypeMismatchException : Exception
{
    private const string ErrorMessageFormat = "Input type {0} does not match handler's generic argument constraint {1}";

    public InputTypeMismatchException(Type inputType, Type handlerGenericParameter) 
        : base(string.Format(ErrorMessageFormat, inputType.Name, handlerGenericParameter.Name)) { }
}