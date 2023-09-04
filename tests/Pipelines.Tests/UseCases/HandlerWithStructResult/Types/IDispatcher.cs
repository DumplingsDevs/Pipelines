namespace Pipelines.Tests.UseCases.HandlerWithStructResult.Types;

public interface IDispatcher
{
    public Task<TResult> SendAsync<TResult>(IInput<TResult> input, CancellationToken token)  where TResult : struct;
}