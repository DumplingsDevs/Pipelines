namespace Pipelines.Builder.Validators.Dispatcher.InputType.Exceptions;

public class DispatcherMethodInputTypeMismatchException : Exception
{
    private const string ErrorMessage = "The namespace of the input type {0} does not match the namespace of the dispatcher method parameter type {1}.";

    public DispatcherMethodInputTypeMismatchException(Type inputType, Type dispatcherType)
        : base(string.Format(ErrorMessage, inputType.FullName, dispatcherType.FullName))
    {
    }
}