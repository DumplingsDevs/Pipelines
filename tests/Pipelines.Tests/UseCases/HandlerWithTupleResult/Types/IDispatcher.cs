namespace Pipelines.Tests.UseCases.HandlerWithTupleResult.Types;

public interface IDispatcher
{
    public (TResult, TResult2) SendAsync<TResult, TResult2>(IInput<TResult, TResult2> input,
        CancellationToken token) where TResult : class where TResult2 : class;
}