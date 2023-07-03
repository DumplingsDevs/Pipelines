namespace Pipelines.Tests.Builder.Validators.Dispatcher.ResultTypes.Types;

public interface IDispatcherWithResult
{
    public Task<TResult> SendAsync<TResult>(ICommandWithResult<TResult> request, CancellationToken token);
}