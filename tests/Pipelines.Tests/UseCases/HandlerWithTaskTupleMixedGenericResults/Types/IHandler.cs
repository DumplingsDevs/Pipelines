namespace Pipelines.Tests.UseCases.HandlerWithTaskTupleMixedGenericResults.Types;

public interface IHandler<in TInput, TResult, TResult2, TResult3> where TInput : IInput<TResult, TResult2>
{
    public (TResult, TResult2, TResult3) HandleAsync(TInput command, CancellationToken token);
}