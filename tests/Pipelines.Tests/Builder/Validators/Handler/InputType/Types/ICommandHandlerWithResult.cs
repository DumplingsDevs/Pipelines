namespace Pipelines.Tests.Builder.Validators.Handler.InputType.Types;

public interface ICommandHandlerWithResult<in TInput, TResult> where TInput : ICommandWithResult<TResult>
{
    public Task<TResult> HandleAsync(TInput command, CancellationToken token);
}