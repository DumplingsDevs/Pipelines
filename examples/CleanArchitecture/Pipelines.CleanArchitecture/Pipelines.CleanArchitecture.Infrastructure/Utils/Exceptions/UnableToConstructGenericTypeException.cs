namespace Pipelines.CleanArchitecture.Infrastructure.Utils.Exceptions;

public class UnableToConstructGenericTypeException : Exception
{
    public UnableToConstructGenericTypeException(string queryType) : base(
        $"Unable to create types based on query type for '{queryType}'")
    { }

    public UnableToConstructGenericTypeException(string queryType, Exception exception) : base(
        $"Unable to create types based on query type for '{queryType}'", exception)
    { }
}