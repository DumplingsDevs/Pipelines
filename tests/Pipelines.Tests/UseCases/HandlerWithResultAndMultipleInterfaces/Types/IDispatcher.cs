namespace Pipelines.Tests.UseCases.HandlerWithResultAndMultipleInterfaces.Types;

public interface IDispatcher
{
    public Task<TResult> SendAsync<TResult>(IInput<TResult> input, CancellationToken token);
}