namespace Pipelines.Tests.UseCases.VoidHandler.Types;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    public void HandleAsync(TCommand command, CancellationToken token);
}