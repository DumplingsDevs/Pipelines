namespace Pipelines.Builder.Validators.CrossValidation.ResultType.Exceptions;

public class ResultTypeMismatchException : Exception
{
    private const string ErrorMessage = "Result type mismatch. Handler: {0}, Dispatcher: {1}";

    public ResultTypeMismatchException(Type handlerType, Type dispatcherType)
        : base(string.Format(ErrorMessage, handlerType.FullName, dispatcherType.FullName))
    { }
}