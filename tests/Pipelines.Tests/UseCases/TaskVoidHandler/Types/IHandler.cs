namespace Pipelines.Tests.UseCases.TaskVoidHandler.Types;

public interface IHandler<in TCommand> where TCommand : IInput
{
    public Task HandleAsync(TCommand command, CancellationToken token);
}