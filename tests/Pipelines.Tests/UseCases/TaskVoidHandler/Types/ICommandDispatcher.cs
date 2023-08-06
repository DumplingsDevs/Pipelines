namespace Pipelines.Tests.UseCases.TaskVoidHandler.Types;

public interface ICommandDispatcher
{
    public Task SendAsync(ICommand command, CancellationToken token);
}