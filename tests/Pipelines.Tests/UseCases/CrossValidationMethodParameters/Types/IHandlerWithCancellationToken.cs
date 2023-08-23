namespace Pipelines.Tests.UseCases.CrossValidationMethodParameters.Types;

public interface IHandlerWithCancellationToken<in TCommand> where TCommand : IInputType
{
    public Task<string> HandleAsync(TCommand command, CancellationToken token);
}