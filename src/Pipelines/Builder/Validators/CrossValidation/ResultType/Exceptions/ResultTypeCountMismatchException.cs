namespace Pipelines.Builder.Validators.CrossValidation.ResultType.Exceptions;

public class ResultTypeCountMismatchException : Exception
{
    private const string ErrorMessage = "Result type count mismatch. Handler: {0}, Dispatcher: {1}";

    public ResultTypeCountMismatchException(Type handlerType, Type dispatcherType)
        : base(string.Format(ErrorMessage, handlerType.FullName, dispatcherType.FullName))
    { }
}
