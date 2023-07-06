namespace Pipelines.Builder.Validators.CrossValidation.ResultType.Exceptions;

public class GenericTypeMismatchException : Exception
{
    private const string ErrorMessage = "Generic type mismatch in result type. Handler: {0}, Dispatcher: {1}";

    public GenericTypeMismatchException(Type handlerType, Type dispatcherType)
        : base(string.Format(ErrorMessage, handlerType.FullName, dispatcherType.FullName))
    { }
}