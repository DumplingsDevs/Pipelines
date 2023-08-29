namespace Pipelines.Tests.UseCases.CrossValidationReturnType.Types;

public interface IDispatcherTaskWithConstrainedResult
{
    public Task<TResult> SendAsync<TResult>(IInputType inputType, CancellationToken token) where TResult : IResultOne;
}