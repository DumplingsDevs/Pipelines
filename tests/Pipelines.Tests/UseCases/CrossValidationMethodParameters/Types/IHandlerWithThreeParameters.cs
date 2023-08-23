namespace Pipelines.Tests.UseCases.CrossValidationMethodParameters.Types;

public interface IHandlerWithThreeParameters<in TCommand> where TCommand : IInputType
{
    public Task<string> HandleAsync(TCommand command, int index, CancellationToken token);
}