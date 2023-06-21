namespace Pipelines.Exceptions;

internal class HandleMethodMismatchException : Exception
{
    private const string ErrorMessage = "Type '{0}' does not have the same handle method signature as the first type '{1}'.";

    public HandleMethodMismatchException(Type type1, Type type2) : base(string.Format(ErrorMessage, type1.Name, type2.Name))
    {
    }
}