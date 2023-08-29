namespace Pipelines.Tests.Builder.Validators.Dispatcher.ResultTypes.Types;

public interface IDispatcherMismatchGenericMethodArgumentAndResultType
{
    public Task SendAsync<TResult>(IInputWithResult<TResult> request, CancellationToken token);
}