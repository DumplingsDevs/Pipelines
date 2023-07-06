namespace Pipelines.Tests.Builder.Validators.CrossValidation.ResultType.Types;

public interface IDispatcherTaskWithDifferentTwoConstraintedResults
{
    public Task<(TResult, TResultTwo)> SendAsync<TResult, TResultTwo>(IInputType inputType)
        where TResult : IResultOne where TResultTwo : class;
}