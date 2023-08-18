namespace Pipelines.Tests.UseCases.HandlerWithTaskWithTuple.Types;

public interface IDispatcher
{
    public Task<(TResult, TResult2)> SendAsync<TResult, TResult2>(IInput<TResult, TResult2> input,
        CancellationToken token);
}