namespace Pipelines.Tests.UseCases.HandlerTwoHandleAsync.Types;

public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult> where TResult : class
{
    public Task<TResult> HandleAsync(TCommand command, CancellationToken token);
}