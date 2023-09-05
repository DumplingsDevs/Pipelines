using Pipelines.CleanArchitecture.Abstractions.Errors;

namespace Pipelines.CleanArchitecture.Abstractions.Commands.Exceptions;

public class CommandValidationFailedException : Exception
{
    public CommandValidationFailedException(List<DetailedErrorDescription> errorMessages) : base("Command structure is invalid")
    {
        ErrorDetails = errorMessages;
    }

    public List<DetailedErrorDescription> ErrorDetails { get; }
}

