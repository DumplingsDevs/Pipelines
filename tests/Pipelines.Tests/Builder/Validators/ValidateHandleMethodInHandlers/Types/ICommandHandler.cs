namespace Pipelines.Tests.Builder.Validators.ValidateHandleMethodInHandlers.Types;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    public Task HandleAsync(TCommand command, CancellationToken token);
}