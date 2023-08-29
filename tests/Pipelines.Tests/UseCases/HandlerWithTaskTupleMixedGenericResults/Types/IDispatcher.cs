namespace Pipelines.Tests.UseCases.HandlerWithTaskTupleMixedGenericResults.Types;

public interface IDispatcher
{
    public (TResult, TResult2, TResult3) SendAsync<TResult, TResult2, TResult3>(IInput<TResult, TResult2> input,
        CancellationToken token);
}