namespace Pipelines.Tests.UseCases.HandlerWithResult.Types;

public interface IHandler<in TCommand, TResult> where TCommand : IInput<TResult>
{
    public Task<TResult> HandleAsync(TCommand command, CancellationToken token);
}