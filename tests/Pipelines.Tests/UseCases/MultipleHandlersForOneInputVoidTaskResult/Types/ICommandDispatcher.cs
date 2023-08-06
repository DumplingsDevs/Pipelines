namespace Pipelines.Tests.UseCases.MultipleHandlersForOneInputVoidTaskResult.Types;

public interface ICommandDispatcher
{
    public Task SendAsync(ICommand command, CancellationToken token);
}