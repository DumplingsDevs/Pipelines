using Pipelines.CleanArchitecture.Abstractions.Errors;

namespace Pipelines.CleanArchitecture.Abstractions.Queries.Exceptions;

public class QueryValidationFailedException : Exception
{
    public QueryValidationFailedException(List<DetailedErrorDescription> errorDetails) : base("Query structure is invalid")
    {
        ErrorDetails = errorDetails;
    }

    public List<DetailedErrorDescription> ErrorDetails { get; }
}