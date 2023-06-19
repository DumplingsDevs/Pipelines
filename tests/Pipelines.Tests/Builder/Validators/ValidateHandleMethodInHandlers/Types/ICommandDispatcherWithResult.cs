namespace Pipelines.Tests.Builder.Validators.ValidateHandleMethodInHandlers.Types;

public interface ICommandDispatcherWithResult
{
    public Task<TResult> SendAsync<TResult>(ICommandWithResult<TResult> commandWithResult, CancellationToken token);
}