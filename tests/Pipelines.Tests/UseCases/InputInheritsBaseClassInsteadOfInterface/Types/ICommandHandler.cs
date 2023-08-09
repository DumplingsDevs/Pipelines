namespace Pipelines.Tests.UseCases.InputInheritsBaseClassInsteadOfInterface.Types;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    public Task HandleAsync(TCommand command, CancellationToken token);
}