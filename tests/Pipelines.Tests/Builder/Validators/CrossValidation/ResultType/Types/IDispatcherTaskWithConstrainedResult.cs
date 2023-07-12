namespace Pipelines.Tests.Builder.Validators.CrossValidation.ResultType.Types;

public interface IDispatcherTaskWithConstrainedResult
{
    public Task<TResult> SendAsync<TResult>(IInputType inputType) where TResult : IResultOne;
}