namespace Pipelines.Builder.Validators.CrossValidation.ResultType.Exceptions;

public class ReturnTypeMismatchException : Exception
{
    private static readonly string ExceptionMessageFormat = "The return type of handler {0} does not match with the return type of dispatcher {1}.";

    public ReturnTypeMismatchException(Type handlerType, Type dispatcherType)
        : base(string.Format(ExceptionMessageFormat, handlerType.FullName, dispatcherType.FullName))
    { }
}