namespace Pipelines.Tests.Builder.Validators.Dispatcher.ResultTypes.Types;

public interface IDispatcherWithResult
{
    public Task<TResult> SendAsync<TResult>(IInputWithResult<TResult> request, CancellationToken token);
}