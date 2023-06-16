namespace Pipelines.Tests.Builder.Validators.ExactlyOneHandleMethodShouldBeDefineds.Types;

public interface ICommandDispatcher
{
    public Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken token);
}