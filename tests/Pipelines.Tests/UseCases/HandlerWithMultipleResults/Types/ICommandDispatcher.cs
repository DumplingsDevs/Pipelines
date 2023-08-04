namespace Pipelines.Tests.UseCases.HandlerWithMultipleResults.Types;

public interface ICommandDispatcher
{
    public (TResult, TResult2) SendAsync<TResult, TResult2>(ICommand<TResult, TResult2> command,
        CancellationToken token) where TResult : class where TResult2 : class;
}