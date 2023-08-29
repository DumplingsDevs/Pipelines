namespace Pipelines.Tests.UseCases.InputWithInheritanceDispatcherWithGenericInput.Types;

public interface IHandler<in TCommand> where TCommand : class, IInput, IMessage
{
    public Task HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}