namespace Pipelines.Tests.Builder.Decorators.Types;

public interface ICommandDispatcher
{
    public Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken token);
}