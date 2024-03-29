namespace Pipelines.Tests.UseCases.MultipleHandleAsyncInHandlerImplementation.Types;

public interface ICommandDispatcher
{
    public Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken token) where TResult : class;
}