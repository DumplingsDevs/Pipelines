namespace Pipelines.Tests.UseCases.HandlerWithTaskWithTwoResults.Types;

public interface ICommandDispatcher
{
    public Task<(TResult, TResult2)> SendAsync<TResult, TResult2>(ICommand<TResult, TResult2> command,
        CancellationToken token) where TResult : class where TResult2 : class;
}