namespace Pipelines.Tests.Builder.Validators.Decorator.ImplementsInterface.Types;

public interface ICommandHandler<in TInput, TResult> where TInput : ICommandWithResult<TResult>
{
    public Task<TResult> HandleAsync(TInput command, CancellationToken token);
}