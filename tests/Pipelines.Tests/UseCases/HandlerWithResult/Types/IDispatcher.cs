namespace Pipelines.Tests.UseCases.HandlerWithResult.Types;

public interface IDispatcher
{
    public Task<TResult> SendAsync<TResult>(IInput<TResult> input, CancellationToken token);
}