namespace Pipelines.Builder.Validators.Shared.CompareTypes.Exceptions;

internal class GenericTypeCountMismatchException : Exception
{
    private const string ErrorMessage =
        "Generic type count mismatch in provided type '{0}' from '{1}' and type '{2}' from '{3}'";

    public GenericTypeCountMismatchException(Type handlerType, Type dispatcherType, Type sourceType1, Type sourceType2)
        : base(string.Format(ErrorMessage, handlerType, sourceType1, dispatcherType, sourceType2))
    { }
}