namespace Pipelines.Tests.UseCases.HandlerWithGenericInputStringResult.Types;

public interface IDispatcher
{
    public Task<TResult> SendAsync<TResult>(IInput<TResult> input, CancellationToken token);
}