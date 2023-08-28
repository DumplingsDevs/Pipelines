namespace Pipelines.Tests.Builder.Validators.Decorator.Constructor.Types;

public interface ICommandHandler<in TInput, TResult> where TInput : ICommandWithResult<TResult>
{
    public Task<TResult> HandleAsync(TInput command, CancellationToken token);
}