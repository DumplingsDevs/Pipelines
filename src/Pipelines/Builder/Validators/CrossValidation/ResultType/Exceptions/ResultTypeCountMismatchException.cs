namespace Pipelines.Builder.Validators.CrossValidation.ResultType.Exceptions;

internal class ResultTypeCountMismatchException : Exception
{
    private const string ErrorMessage = "Result type count mismatch. Handler: {0}, Dispatcher: {1}";

    internal ResultTypeCountMismatchException(Type handlerType, Type dispatcherType)
        : base(string.Format(ErrorMessage, handlerType.FullName, dispatcherType.FullName))
    { }
}
