namespace Pipelines.Tests.UseCases.CrossValidationReturnType.Types;

public interface IDispatcherTaskWithDifferentTwoConstraintedResults
{
    public Task<(TResult, TResultTwo)> SendAsync<TResult, TResultTwo>(IInputType inputType, CancellationToken token)
        where TResult : class, IResultOne where TResultTwo : struct;
}