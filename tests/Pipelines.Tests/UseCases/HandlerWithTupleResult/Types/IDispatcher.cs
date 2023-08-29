namespace Pipelines.Tests.UseCases.HandlerWithTupleResult.Types;

public interface IDispatcher
{
    public (TResult, TResult2) Send<TResult, TResult2>(IInput<TResult, TResult2> input);
}