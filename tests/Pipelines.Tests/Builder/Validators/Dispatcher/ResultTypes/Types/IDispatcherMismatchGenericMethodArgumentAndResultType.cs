namespace Pipelines.Tests.Builder.Validators.Dispatcher.ResultTypes.Types;

public interface IDispatcherMismatchGenericMethodArgumentAndResultType
{
    public Task SendAsync<TResult>(ICommandWithResult<TResult> request, CancellationToken token);
}