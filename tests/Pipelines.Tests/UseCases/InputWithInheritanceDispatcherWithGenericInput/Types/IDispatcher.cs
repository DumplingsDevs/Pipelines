namespace Pipelines.Tests.UseCases.InputWithInheritanceDispatcherWithGenericInput.Types;

public interface IDispatcher
{
    Task SendAsync<TInput>(TInput command, CancellationToken cancellationToken = default) where TInput : class, IInput;
}