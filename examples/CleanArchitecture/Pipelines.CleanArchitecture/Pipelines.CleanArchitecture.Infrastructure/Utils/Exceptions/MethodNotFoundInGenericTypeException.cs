namespace Pipelines.CleanArchitecture.Infrastructure.Utils.Exceptions;

public class MethodNotFoundInGenericTypeException : Exception
{
    public MethodNotFoundInGenericTypeException(string queryType, string methodName) : base(
        $"Query Handler does not contains method {methodName}. Query type '{queryType}'")
    {
    }
}