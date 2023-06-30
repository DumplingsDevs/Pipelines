namespace Pipelines.Tests.Builder.Validators.Handler.InputType.Types;

public interface ICommandHandlerWithResult<in TCommand, TResult> where TCommand : ICommandWithResult<TResult>
{
    public Task<TResult> HandleAsync(TCommand command, CancellationToken token);
}