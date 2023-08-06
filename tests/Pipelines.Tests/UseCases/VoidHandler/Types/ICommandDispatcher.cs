namespace Pipelines.Tests.UseCases.VoidHandler.Types;

public interface ICommandDispatcher
{
    public void SendAsync(ICommand command, CancellationToken token);
}