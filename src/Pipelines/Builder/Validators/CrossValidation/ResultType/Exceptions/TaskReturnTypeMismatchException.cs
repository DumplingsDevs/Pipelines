namespace Pipelines.Builder.Validators.CrossValidation.ResultType.Exceptions;

public class TaskReturnTypeMismatchException: Exception
{
    private static readonly string ExceptionMessageFormat = "One method returns a Task<> type while the other does not. Handler: {0}, Dispatcher: {1}.";

    internal TaskReturnTypeMismatchException(Type handlerType, Type dispatcherType)
        : base(string.Format(ExceptionMessageFormat, handlerType.FullName, dispatcherType.FullName))
    { }
}