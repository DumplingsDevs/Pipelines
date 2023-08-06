namespace Pipelines.Builder.Validators.CrossValidation.ResultType.Exceptions;

internal class VoidAndValueMethodMismatchException : Exception
{
    private static readonly string ExceptionMessageFormat = "The return type of handler {0} does not match with the return type of dispatcher {1}.";

    internal VoidAndValueMethodMismatchException(Type handlerType, Type dispatcherType)
        : base(string.Format(ExceptionMessageFormat, handlerType.FullName, dispatcherType.FullName))
    { }
}