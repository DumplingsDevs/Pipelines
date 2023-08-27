namespace Pipelines.Tests.UseCases.CrossValidationReturnType.Types;

public interface IDispatcherTaskWithDifferentTwoConstraintedResults
{
    public Task<(TResult, TResultTwo)> SendAsync<TResult, TResultTwo>(IInputType inputType, CancellationToken token)
        where TResult : IResultOne where TResultTwo : struct;
}