namespace Pipelines.Tests.UseCases.TaskVoidHandler.Types;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    public Task HandleAsync(TCommand command, CancellationToken token);
}