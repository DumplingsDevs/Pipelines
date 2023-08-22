namespace Pipelines.Tests.UseCases.CrossValidationReturnType.Types;

public interface IHandlerStringResult<in TCommand> where TCommand : IInputType
{
    public string HandleAsync(TCommand command, CancellationToken token);
}