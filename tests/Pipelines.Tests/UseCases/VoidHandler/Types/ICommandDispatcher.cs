namespace Pipelines.Tests.UseCases.VoidHandler.Types;

public interface ICommandDispatcher
{
    public Task SendAsync(ICommand command, CancellationToken token);
}