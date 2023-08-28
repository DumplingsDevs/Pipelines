namespace Pipelines.Tests.Builder.Decorators.Types;

public interface ICommandHandler<in TInput, TResult> where TInput : ICommand<TResult>
{
    public Task<TResult> HandleAsync(TInput command, CancellationToken token);
}