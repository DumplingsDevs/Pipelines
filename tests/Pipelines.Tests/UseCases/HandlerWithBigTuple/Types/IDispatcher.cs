namespace Pipelines.Tests.UseCases.HandlerWithBigTuple.Types;

public interface IDispatcher
{
    public (TResult, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7) SendAsync<TResult, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>(IInput<TResult, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7> input,
        CancellationToken token);
}