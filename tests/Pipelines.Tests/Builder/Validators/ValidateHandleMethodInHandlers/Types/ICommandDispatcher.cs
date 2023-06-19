namespace Pipelines.Tests.Builder.Validators.ValidateHandleMethodInHandlers.Types;

public interface ICommandDispatcher
{
    public Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken token);
}