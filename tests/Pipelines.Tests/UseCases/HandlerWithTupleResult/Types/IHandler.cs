namespace Pipelines.Tests.UseCases.HandlerWithTupleResult.Types;

public interface IHandler<in TInput, TResult, TResult2> where TInput : IInput<TResult, TResult2>
    where TResult : class where TResult2 : class
{
    public (TResult, TResult2) HandleAsync(TInput input);
}