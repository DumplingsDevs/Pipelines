namespace Pipelines.Tests.Builder.Validators.Decorator.ImplementsInterface.Types;

public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommandWithResult<TResult>
{
    public Task<TResult> HandleAsync(TCommand command, CancellationToken token);
}