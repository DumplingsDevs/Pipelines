namespace Pipelines.Tests.Builder.Validators.Dispatcher.ResultTypes.Types;

public interface IDispatcherMismatchTwoResultExpectedOne
{
    public Task<(TResult, TResult2)> SendAsync<TResult,TResult2>(IInputWithResult<TResult> request, CancellationToken token);
}