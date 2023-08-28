namespace Pipelines.Tests.UseCases.HandlerWithTaskTupleMixedGenericResults.Types;

public interface IHandler<in TInput, TResult, TResult2, TResult3> where TInput : IInput<TResult, TResult2>
    where TResult : class where TResult2 : class where TResult3 : class
{
    public (TResult, TResult2, TResult3) HandleAsync(TInput command, CancellationToken token);
}