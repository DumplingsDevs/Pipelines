namespace Pipelines.Tests.UseCases.VoidHandler.Types;

public interface IHandler<in TCommand> where TCommand : IInput
{
    public void HandleAsync(TCommand command, CancellationToken token);
}