namespace Pipelines.Builder.Validators.Shared.CompareTypes.Exceptions;

public class GenericTypeCountMismatchException : Exception
{
    private const string ErrorMessage = "Generic type count mismatch in result type. Handler: {0}, Dispatcher: {1}";

    public GenericTypeCountMismatchException(Type handlerType, Type dispatcherType)
        : base(string.Format(ErrorMessage, handlerType.FullName, dispatcherType.FullName))
    { }
}