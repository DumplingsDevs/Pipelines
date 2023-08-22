namespace Pipelines.Tests.UseCases.CrossValidationReturnType.Types;

public interface IHandlerTaskStringResult<in TCommand> where TCommand : IInputType
{
    public Task<string> HandleAsync(TCommand command, CancellationToken token);
}