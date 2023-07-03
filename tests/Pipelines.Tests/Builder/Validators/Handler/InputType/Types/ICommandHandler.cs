namespace Pipelines.Tests.Builder.Validators.Handler.InputType.Types;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    public Task HandleAsync(TCommand command, CancellationToken token);
}