namespace Pipelines.Tests.Builder.Validators.ValidateHandleMethodInHandlers.Types;

public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
{
    public Task<TResult> HandleAsync(TCommand command, CancellationToken token);
}