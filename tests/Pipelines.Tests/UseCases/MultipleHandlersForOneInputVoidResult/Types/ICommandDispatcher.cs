namespace Pipelines.Tests.UseCases.MultipleHandlersForOneInputVoidResult.Types;

public interface ICommandDispatcher
{
    public void SendAsync(ICommand command, CancellationToken token);
}