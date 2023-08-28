namespace Pipelines.Tests.UseCases.InputWithInheritanceDispatcherWithGenericInput.Types;

public interface IHandler<in TCommand> where TCommand : class, IInput
{
    public Task HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}