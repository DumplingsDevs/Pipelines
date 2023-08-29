namespace Pipelines.Tests.UseCases.HandlerWithTupleResult.Types;

public interface IHandler<in TInput, TResult, TResult2> where TInput : IInput<TResult, TResult2>
{
    public (TResult, TResult2) HandleAsync(TInput input);
}