namespace Pipelines.Exceptions;

public class InputNotSupportedByDispatcherException : Exception
{
    private static string ErrorMessage = "InputType {0} is not supported by Dispatcher {1}";

    public InputNotSupportedByDispatcherException(Type inputType, Type dispatcherInterfaceType) : base(
        string.Format(ErrorMessage, inputType.FullName, dispatcherInterfaceType.Name))
    {
    }
}