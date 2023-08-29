namespace Pipelines.Tests.UseCases.CrossValidationReturnType.Types;

public interface IDispatcherTaskWithTwoConstrainedResults
{
    public Task<(TResult, TResultTwo)> SendAsync<TResult, TResultTwo>(IInputType inputType, CancellationToken token)
        where TResult : IResultOne where TResultTwo : IResultTwo;
}