namespace Pipelines.Tests.UseCases.HandlerWithMultipleParameters.Types;

public interface IDispatcher
{
    public Task<TResult> SendAsync<TResult>(IInput<TResult> input, CancellationToken t, bool canDoSomething,
        Dictionary<string, string> dictionary) where TResult : class;
}