namespace Pipelines.Tests.UseCases.HandlerWithResult.Types;

public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
{
    public Task<TResult> HandleAsync(TCommand command, CancellationToken token);
}