namespace Pipelines.Tests.Builder.Validators.CrossValidation.ResultType.Types;

public interface IDispatcherTaskWithClassConstraintedResults
{
    public Task<(TResult, TResultTwo)> SendAsync<TResult, TResultTwo>(IInputType inputType)
        where TResult : IResultOne where TResultTwo : class;
}